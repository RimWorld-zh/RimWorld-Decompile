using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_DoBill : WorkGiver_Scanner
	{
		private class DefCountList
		{
			private List<ThingDef> defs = new List<ThingDef>();

			private List<float> counts = new List<float>();

			public int Count
			{
				get
				{
					return this.defs.Count;
				}
			}

			public float this[ThingDef def]
			{
				get
				{
					int num = this.defs.IndexOf(def);
					if (num < 0)
					{
						return 0f;
					}
					return this.counts[num];
				}
				set
				{
					int num = this.defs.IndexOf(def);
					if (num < 0)
					{
						this.defs.Add(def);
						this.counts.Add(value);
						num = this.defs.Count - 1;
					}
					else
					{
						this.counts[num] = value;
					}
					this.CheckRemove(num);
				}
			}

			public float GetCount(int index)
			{
				return this.counts[index];
			}

			public void SetCount(int index, float val)
			{
				this.counts[index] = val;
				this.CheckRemove(index);
			}

			public ThingDef GetDef(int index)
			{
				return this.defs[index];
			}

			private void CheckRemove(int index)
			{
				if (this.counts[index] == 0.0)
				{
					this.counts.RemoveAt(index);
					this.defs.RemoveAt(index);
				}
			}

			public void Clear()
			{
				this.defs.Clear();
				this.counts.Clear();
			}

			public void GenerateFrom(List<Thing> things)
			{
				this.Clear();
				for (int i = 0; i < things.Count; i++)
				{
					DefCountList defCountList;
					DefCountList obj = defCountList = this;
					ThingDef def;
					ThingDef def2 = def = things[i].def;
					float num = defCountList[def];
					obj[def2] = num + (float)things[i].stackCount;
				}
			}
		}

		private List<ThingAmount> chosenIngThings = new List<ThingAmount>();

		private static readonly IntRange ReCheckFailedBillTicksRange = new IntRange(500, 600);

		private static string MissingMaterialsTranslated;

		private static string MissingSkillTranslated;

		private static List<Thing> relevantThings = new List<Thing>();

		private static HashSet<Thing> processedThings = new HashSet<Thing>();

		private static List<Thing> newRelevantThings = new List<Thing>();

		private static List<IngredientCount> ingredientsOrdered = new List<IngredientCount>();

		private static List<Thing> tmpMedicine = new List<Thing>();

		private static DefCountList availableCounts = new DefCountList();

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				if (base.def.fixedBillGiverDefs != null && base.def.fixedBillGiverDefs.Count == 1)
				{
					return ThingRequest.ForDef(base.def.fixedBillGiverDefs[0]);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver);
			}
		}

		public WorkGiver_DoBill()
		{
			if (WorkGiver_DoBill.MissingSkillTranslated == null)
			{
				WorkGiver_DoBill.MissingSkillTranslated = "MissingSkill".Translate();
			}
			if (WorkGiver_DoBill.MissingMaterialsTranslated == null)
			{
				WorkGiver_DoBill.MissingMaterialsTranslated = "MissingMaterials".Translate();
			}
		}

		public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
		{
			IBillGiver billGiver = thing as IBillGiver;
			if (billGiver != null && this.ThingIsUsableBillGiver(thing) && billGiver.CurrentlyUsable() && billGiver.BillStack.AnyShouldDoNow && pawn.CanReserve(thing, 1, -1, null, forced) && !thing.IsBurning() && !thing.IsForbidden(pawn))
			{
				if (!pawn.CanReach(thing.InteractionCell, PathEndMode.OnCell, Danger.Some, false, TraverseMode.ByPawn))
				{
					return null;
				}
				billGiver.BillStack.RemoveIncompletableBills();
				return this.StartOrResumeBillJob(pawn, billGiver);
			}
			return null;
		}

		private static UnfinishedThing ClosestUnfinishedThingForBill(Pawn pawn, Bill_ProductionWithUft bill)
		{
			Predicate<Thing> validator;
			Predicate<Thing> predicate = validator = (Predicate<Thing>)((Thing t) => !t.IsForbidden(pawn) && ((UnfinishedThing)t).Recipe == bill.recipe && ((UnfinishedThing)t).Creator == pawn && ((UnfinishedThing)t).ingredients.TrueForAll((Predicate<Thing>)((Thing x) => bill.IsFixedOrAllowedIngredient(x.def))) && pawn.CanReserve(t, 1, -1, null, false));
			return (UnfinishedThing)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(bill.recipe.unfinishedThingDef), PathEndMode.InteractionCell, TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		private static Job FinishUftJob(Pawn pawn, UnfinishedThing uft, Bill_ProductionWithUft bill)
		{
			if (uft.Creator != pawn)
			{
				Log.Error("Tried to get FinishUftJob for " + pawn + " finishing " + uft + " but its creator is " + uft.Creator);
				return null;
			}
			Job job = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, bill.billStack.billGiver, uft);
			if (job != null && job.targetA.Thing != uft)
			{
				return job;
			}
			Job job2 = new Job(JobDefOf.DoBill, (Thing)bill.billStack.billGiver);
			job2.bill = bill;
			job2.targetQueueB = new List<LocalTargetInfo>
			{
				(Thing)uft
			};
			job2.countQueue = new List<int>
			{
				1
			};
			job2.haulMode = HaulMode.ToCellNonStorage;
			return job2;
		}

		private Job StartOrResumeBillJob(Pawn pawn, IBillGiver giver)
		{
			for (int i = 0; i < giver.BillStack.Count; i++)
			{
				Bill bill = giver.BillStack[i];
				if ((bill.recipe.requiredGiverWorkType == null || bill.recipe.requiredGiverWorkType == base.def.workType) && (Find.TickManager.TicksGame >= bill.lastIngredientSearchFailTicks + WorkGiver_DoBill.ReCheckFailedBillTicksRange.RandomInRange || FloatMenuMakerMap.making))
				{
					bill.lastIngredientSearchFailTicks = 0;
					if (bill.ShouldDoNow() && bill.PawnAllowedToStartAnew(pawn))
					{
						if (bill.recipe.PawnSatisfiesSkillRequirements(pawn))
						{
							Bill_ProductionWithUft bill_ProductionWithUft = bill as Bill_ProductionWithUft;
							if (bill_ProductionWithUft != null)
							{
								if (bill_ProductionWithUft.BoundUft != null)
								{
									if (bill_ProductionWithUft.BoundWorker == pawn && pawn.CanReserveAndReach((Thing)bill_ProductionWithUft.BoundUft, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false) && !bill_ProductionWithUft.BoundUft.IsForbidden(pawn))
									{
										return WorkGiver_DoBill.FinishUftJob(pawn, bill_ProductionWithUft.BoundUft, bill_ProductionWithUft);
									}
									continue;
								}
								UnfinishedThing unfinishedThing = WorkGiver_DoBill.ClosestUnfinishedThingForBill(pawn, bill_ProductionWithUft);
								if (unfinishedThing != null)
								{
									return WorkGiver_DoBill.FinishUftJob(pawn, unfinishedThing, bill_ProductionWithUft);
								}
							}
							if (!WorkGiver_DoBill.TryFindBestBillIngredients(bill, pawn, (Thing)giver, this.chosenIngThings))
							{
								if (!FloatMenuMakerMap.making)
								{
									bill.lastIngredientSearchFailTicks = Find.TickManager.TicksGame;
								}
								else
								{
									JobFailReason.Is(WorkGiver_DoBill.MissingMaterialsTranslated);
								}
								continue;
							}
							return this.TryStartNewDoBillJob(pawn, bill, giver);
						}
						JobFailReason.Is(WorkGiver_DoBill.MissingSkillTranslated);
					}
				}
			}
			return null;
		}

		private Job TryStartNewDoBillJob(Pawn pawn, Bill bill, IBillGiver giver)
		{
			Job job = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, giver, null);
			if (job != null)
			{
				return job;
			}
			Job job2 = new Job(JobDefOf.DoBill, (Thing)giver);
			job2.targetQueueB = new List<LocalTargetInfo>(this.chosenIngThings.Count);
			job2.countQueue = new List<int>(this.chosenIngThings.Count);
			for (int i = 0; i < this.chosenIngThings.Count; i++)
			{
				List<LocalTargetInfo> targetQueueB = job2.targetQueueB;
				ThingAmount thingAmount = this.chosenIngThings[i];
				targetQueueB.Add(thingAmount.thing);
				List<int> countQueue = job2.countQueue;
				ThingAmount thingAmount2 = this.chosenIngThings[i];
				countQueue.Add(thingAmount2.count);
			}
			job2.haulMode = HaulMode.ToCellNonStorage;
			job2.bill = bill;
			return job2;
		}

		private bool ThingIsUsableBillGiver(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			Corpse corpse = thing as Corpse;
			Pawn pawn2 = null;
			if (corpse != null)
			{
				pawn2 = corpse.InnerPawn;
			}
			if (base.def.fixedBillGiverDefs != null && base.def.fixedBillGiverDefs.Contains(thing.def))
			{
				return true;
			}
			if (pawn != null)
			{
				if (base.def.billGiversAllHumanlikes && pawn.RaceProps.Humanlike)
				{
					return true;
				}
				if (base.def.billGiversAllMechanoids && pawn.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (base.def.billGiversAllAnimals && pawn.RaceProps.Animal)
				{
					return true;
				}
			}
			if (corpse != null && pawn2 != null)
			{
				if (base.def.billGiversAllHumanlikesCorpses && pawn2.RaceProps.Humanlike)
				{
					return true;
				}
				if (base.def.billGiversAllMechanoidsCorpses && pawn2.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (base.def.billGiversAllAnimalsCorpses && pawn2.RaceProps.Animal)
				{
					return true;
				}
			}
			return false;
		}

		private static bool TryFindBestBillIngredients(Bill bill, Pawn pawn, Thing billGiver, List<ThingAmount> chosen)
		{
			chosen.Clear();
			WorkGiver_DoBill.newRelevantThings.Clear();
			if (bill.recipe.ingredients.Count == 0)
			{
				return true;
			}
			IntVec3 rootCell = WorkGiver_DoBill.GetBillGiverRootCell(billGiver, pawn);
			Region rootReg = rootCell.GetRegion(pawn.Map, RegionType.Set_Passable);
			if (rootReg == null)
			{
				return false;
			}
			WorkGiver_DoBill.MakeIngredientsListInProcessingOrder(WorkGiver_DoBill.ingredientsOrdered, bill);
			WorkGiver_DoBill.relevantThings.Clear();
			WorkGiver_DoBill.processedThings.Clear();
			bool foundAll = false;
			Predicate<Thing> baseValidator = (Predicate<Thing>)((Thing t) => t.Spawned && !t.IsForbidden(pawn) && (float)(t.Position - billGiver.Position).LengthHorizontalSquared < bill.ingredientSearchRadius * bill.ingredientSearchRadius && bill.IsFixedOrAllowedIngredient(t) && bill.recipe.ingredients.Any((Predicate<IngredientCount>)((IngredientCount ingNeed) => ingNeed.filter.Allows(t))) && pawn.CanReserve(t, 1, -1, null, false));
			bool billGiverIsPawn = billGiver is Pawn;
			if (billGiverIsPawn)
			{
				WorkGiver_DoBill.AddEveryMedicineToRelevantThings(pawn, billGiver, WorkGiver_DoBill.relevantThings, baseValidator, pawn.Map);
				if (WorkGiver_DoBill.TryFindBestBillIngredientsInSet(WorkGiver_DoBill.relevantThings, bill, chosen))
				{
					return true;
				}
			}
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			RegionEntryPredicate entryCondition = (RegionEntryPredicate)((Region from, Region r) => r.Allows(traverseParams, false));
			int adjacentRegionsAvailable = rootReg.Neighbors.Count((Func<Region, bool>)((Region region) => entryCondition(rootReg, region)));
			int regionsProcessed = 0;
			WorkGiver_DoBill.processedThings.AddRange(WorkGiver_DoBill.relevantThings);
			RegionProcessor regionProcessor = (RegionProcessor)delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (!WorkGiver_DoBill.processedThings.Contains(thing) && ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, pawn) && baseValidator(thing) && (!thing.def.IsMedicine || !billGiverIsPawn))
					{
						WorkGiver_DoBill.newRelevantThings.Add(thing);
						WorkGiver_DoBill.processedThings.Add(thing);
					}
				}
				regionsProcessed++;
				if (WorkGiver_DoBill.newRelevantThings.Count > 0 && regionsProcessed > adjacentRegionsAvailable)
				{
					Comparison<Thing> comparison = (Comparison<Thing>)delegate(Thing t1, Thing t2)
					{
						float num = (float)(t1.Position - rootCell).LengthHorizontalSquared;
						float value = (float)(t2.Position - rootCell).LengthHorizontalSquared;
						return num.CompareTo(value);
					};
					WorkGiver_DoBill.newRelevantThings.Sort(comparison);
					WorkGiver_DoBill.relevantThings.AddRange(WorkGiver_DoBill.newRelevantThings);
					WorkGiver_DoBill.newRelevantThings.Clear();
					if (WorkGiver_DoBill.TryFindBestBillIngredientsInSet(WorkGiver_DoBill.relevantThings, bill, chosen))
					{
						bool foundAll2 = true;
						return true;
					}
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(rootReg, entryCondition, regionProcessor, 99999, RegionType.Set_Passable);
			WorkGiver_DoBill.relevantThings.Clear();
			WorkGiver_DoBill.newRelevantThings.Clear();
			return foundAll;
		}

		private static IntVec3 GetBillGiverRootCell(Thing billGiver, Pawn forPawn)
		{
			Building building = billGiver as Building;
			if (building != null)
			{
				if (building.def.hasInteractionCell)
				{
					return building.InteractionCell;
				}
				Log.Error("Tried to find bill ingredients for " + billGiver + " which has no interaction cell.");
				return forPawn.Position;
			}
			return billGiver.Position;
		}

		private static void AddEveryMedicineToRelevantThings(Pawn pawn, Thing billGiver, List<Thing> relevantThings, Predicate<Thing> baseValidator, Map map)
		{
			MedicalCareCategory medicalCareCategory = WorkGiver_DoBill.GetMedicalCareCategory(billGiver);
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine);
			WorkGiver_DoBill.tmpMedicine.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (medicalCareCategory.AllowsMedicine(thing.def) && baseValidator(thing) && pawn.CanReach(thing, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					WorkGiver_DoBill.tmpMedicine.Add(thing);
				}
			}
			WorkGiver_DoBill.tmpMedicine.SortBy((Func<Thing, float>)((Thing x) => (float)(0.0 - x.GetStatValue(StatDefOf.MedicalPotency, true))), (Func<Thing, int>)((Thing x) => x.Position.DistanceToSquared(billGiver.Position)));
			relevantThings.AddRange(WorkGiver_DoBill.tmpMedicine);
			WorkGiver_DoBill.tmpMedicine.Clear();
		}

		private static MedicalCareCategory GetMedicalCareCategory(Thing billGiver)
		{
			Pawn pawn = billGiver as Pawn;
			if (pawn != null && pawn.playerSettings != null)
			{
				return pawn.playerSettings.medCare;
			}
			return MedicalCareCategory.Best;
		}

		private static void MakeIngredientsListInProcessingOrder(List<IngredientCount> ingredientsOrdered, Bill bill)
		{
			ingredientsOrdered.Clear();
			if (bill.recipe.productHasIngredientStuff)
			{
				ingredientsOrdered.Add(bill.recipe.ingredients[0]);
			}
			for (int i = 0; i < bill.recipe.ingredients.Count; i++)
			{
				if (((!bill.recipe.productHasIngredientStuff) ? 1 : i) != 0)
				{
					IngredientCount ingredientCount = bill.recipe.ingredients[i];
					if (ingredientCount.IsFixedIngredient)
					{
						ingredientsOrdered.Add(ingredientCount);
					}
				}
			}
			for (int j = 0; j < bill.recipe.ingredients.Count; j++)
			{
				IngredientCount item = bill.recipe.ingredients[j];
				if (!ingredientsOrdered.Contains(item))
				{
					ingredientsOrdered.Add(item);
				}
			}
		}

		private static bool TryFindBestBillIngredientsInSet(List<Thing> availableThings, Bill bill, List<ThingAmount> chosen)
		{
			if (bill.recipe.allowMixingIngredients)
			{
				return WorkGiver_DoBill.TryFindBestBillIngredientsInSet_AllowMix(availableThings, bill, chosen);
			}
			return WorkGiver_DoBill.TryFindBestBillIngredientsInSet_NoMix(availableThings, bill, chosen);
		}

		private static bool TryFindBestBillIngredientsInSet_NoMix(List<Thing> availableThings, Bill bill, List<ThingAmount> chosen)
		{
			RecipeDef recipe = bill.recipe;
			chosen.Clear();
			WorkGiver_DoBill.availableCounts.Clear();
			WorkGiver_DoBill.availableCounts.GenerateFrom(availableThings);
			for (int i = 0; i < WorkGiver_DoBill.ingredientsOrdered.Count; i++)
			{
				IngredientCount ingredientCount = recipe.ingredients[i];
				bool flag = false;
				for (int j = 0; j < WorkGiver_DoBill.availableCounts.Count; j++)
				{
					float num = (float)ingredientCount.CountRequiredOfFor(WorkGiver_DoBill.availableCounts.GetDef(j), bill.recipe);
					if (!(num > WorkGiver_DoBill.availableCounts.GetCount(j)) && ingredientCount.filter.Allows(WorkGiver_DoBill.availableCounts.GetDef(j)) && (ingredientCount.IsFixedIngredient || bill.ingredientFilter.Allows(WorkGiver_DoBill.availableCounts.GetDef(j))))
					{
						for (int k = 0; k < availableThings.Count; k++)
						{
							if (availableThings[k].def == WorkGiver_DoBill.availableCounts.GetDef(j))
							{
								int num2 = availableThings[k].stackCount - ThingAmount.CountUsed(chosen, availableThings[k]);
								if (num2 > 0)
								{
									int num3 = Mathf.Min(Mathf.FloorToInt(num), num2);
									ThingAmount.AddToList(chosen, availableThings[k], num3);
									num -= (float)num3;
									if (num < 0.0010000000474974513)
									{
										flag = true;
										float count = WorkGiver_DoBill.availableCounts.GetCount(j);
										count -= (float)ingredientCount.CountRequiredOfFor(WorkGiver_DoBill.availableCounts.GetDef(j), bill.recipe);
										WorkGiver_DoBill.availableCounts.SetCount(j, count);
										break;
									}
								}
							}
						}
						if (flag)
							break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		private static bool TryFindBestBillIngredientsInSet_AllowMix(List<Thing> availableThings, Bill bill, List<ThingAmount> chosen)
		{
			chosen.Clear();
			for (int i = 0; i < bill.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = bill.recipe.ingredients[i];
				float num = ingredientCount.GetBaseCount();
				for (int j = 0; j < availableThings.Count; j++)
				{
					Thing thing = availableThings[j];
					if (ingredientCount.filter.Allows(thing) && (ingredientCount.IsFixedIngredient || bill.ingredientFilter.Allows(thing)))
					{
						float num2 = bill.recipe.IngredientValueGetter.ValuePerUnitOf(thing.def);
						int num3 = Mathf.Min(Mathf.CeilToInt(num / num2), thing.stackCount);
						ThingAmount.AddToList(chosen, thing, num3);
						num -= (float)num3 * num2;
						if (num <= 9.9999997473787516E-05)
							break;
					}
				}
				if (num > 9.9999997473787516E-05)
				{
					return false;
				}
			}
			return true;
		}
	}
}
