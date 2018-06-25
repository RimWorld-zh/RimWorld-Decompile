using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200013E RID: 318
	public class WorkGiver_DoBill : WorkGiver_Scanner
	{
		// Token: 0x04000317 RID: 791
		private List<ThingCount> chosenIngThings = new List<ThingCount>();

		// Token: 0x04000318 RID: 792
		private static readonly IntRange ReCheckFailedBillTicksRange = new IntRange(500, 600);

		// Token: 0x04000319 RID: 793
		private static string MissingMaterialsTranslated;

		// Token: 0x0400031A RID: 794
		private static List<Thing> relevantThings = new List<Thing>();

		// Token: 0x0400031B RID: 795
		private static HashSet<Thing> processedThings = new HashSet<Thing>();

		// Token: 0x0400031C RID: 796
		private static List<Thing> newRelevantThings = new List<Thing>();

		// Token: 0x0400031D RID: 797
		private static List<IngredientCount> ingredientsOrdered = new List<IngredientCount>();

		// Token: 0x0400031E RID: 798
		private static List<Thing> tmpMedicine = new List<Thing>();

		// Token: 0x0400031F RID: 799
		private static WorkGiver_DoBill.DefCountList availableCounts = new WorkGiver_DoBill.DefCountList();

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x00043364 File Offset: 0x00041764
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0004337C File Offset: 0x0004177C
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Some;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x00043394 File Offset: 0x00041794
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				ThingRequest result;
				if (this.def.fixedBillGiverDefs != null && this.def.fixedBillGiverDefs.Count == 1)
				{
					result = ThingRequest.ForDef(this.def.fixedBillGiverDefs[0]);
				}
				else
				{
					result = ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver);
				}
				return result;
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x000433F2 File Offset: 0x000417F2
		public static void ResetStaticData()
		{
			WorkGiver_DoBill.MissingMaterialsTranslated = "MissingMaterials".Translate();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00043404 File Offset: 0x00041804
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			bool result;
			if (base.ShouldSkip(pawn, forced))
			{
				result = true;
			}
			else
			{
				List<Thing> list = pawn.Map.listerThings.ThingsMatching(this.PotentialWorkThingRequest);
				for (int i = 0; i < list.Count; i++)
				{
					if (this.HasJobOnThing(pawn, list[i], forced))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0004347C File Offset: 0x0004187C
		public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
		{
			IBillGiver billGiver = thing as IBillGiver;
			if (billGiver != null && this.ThingIsUsableBillGiver(thing) && billGiver.BillStack.AnyShouldDoNow && billGiver.UsableForBillsAfterFueling())
			{
				LocalTargetInfo target = thing;
				if (pawn.CanReserve(target, 1, -1, null, forced) && !thing.IsBurning() && !thing.IsForbidden(pawn))
				{
					CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
					if (compRefuelable == null || compRefuelable.HasFuel)
					{
						billGiver.BillStack.RemoveIncompletableBills();
						return this.StartOrResumeBillJob(pawn, billGiver);
					}
					if (!RefuelWorkGiverUtility.CanRefuel(pawn, thing, forced))
					{
						return null;
					}
					return RefuelWorkGiverUtility.RefuelJob(pawn, thing, forced, null, null);
				}
			}
			return null;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0004355C File Offset: 0x0004195C
		private static UnfinishedThing ClosestUnfinishedThingForBill(Pawn pawn, Bill_ProductionWithUft bill)
		{
			Predicate<Thing> predicate = (Thing t) => !t.IsForbidden(pawn) && ((UnfinishedThing)t).Recipe == bill.recipe && ((UnfinishedThing)t).Creator == pawn && ((UnfinishedThing)t).ingredients.TrueForAll((Thing x) => bill.IsFixedOrAllowedIngredient(x.def)) && pawn.CanReserve(t, 1, -1, null, false);
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			ThingRequest thingReq = ThingRequest.ForDef(bill.recipe.unfinishedThingDef);
			PathEndMode peMode = PathEndMode.InteractionCell;
			TraverseParms traverseParams = TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false);
			Predicate<Thing> validator = predicate;
			return (UnfinishedThing)GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00043604 File Offset: 0x00041A04
		private static Job FinishUftJob(Pawn pawn, UnfinishedThing uft, Bill_ProductionWithUft bill)
		{
			Job result;
			if (uft.Creator != pawn)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to get FinishUftJob for ",
					pawn,
					" finishing ",
					uft,
					" but its creator is ",
					uft.Creator
				}), false);
				result = null;
			}
			else
			{
				Job job = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, bill.billStack.billGiver, uft);
				if (job != null && job.targetA.Thing != uft)
				{
					result = job;
				}
				else
				{
					result = new Job(JobDefOf.DoBill, (Thing)bill.billStack.billGiver)
					{
						bill = bill,
						targetQueueB = new List<LocalTargetInfo>
						{
							uft
						},
						countQueue = new List<int>
						{
							1
						},
						haulMode = HaulMode.ToCellNonStorage
					};
				}
			}
			return result;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x000436F8 File Offset: 0x00041AF8
		private Job StartOrResumeBillJob(Pawn pawn, IBillGiver giver)
		{
			for (int i = 0; i < giver.BillStack.Count; i++)
			{
				Bill bill = giver.BillStack[i];
				if (bill.recipe.requiredGiverWorkType == null || bill.recipe.requiredGiverWorkType == this.def.workType)
				{
					if (Find.TickManager.TicksGame >= bill.lastIngredientSearchFailTicks + WorkGiver_DoBill.ReCheckFailedBillTicksRange.RandomInRange || FloatMenuMakerMap.makingFor == pawn)
					{
						bill.lastIngredientSearchFailTicks = 0;
						if (bill.ShouldDoNow())
						{
							if (bill.PawnAllowedToStartAnew(pawn))
							{
								SkillRequirement skillRequirement = bill.recipe.FirstSkillRequirementPawnDoesntSatisfy(pawn);
								if (skillRequirement == null)
								{
									Bill_ProductionWithUft bill_ProductionWithUft = bill as Bill_ProductionWithUft;
									if (bill_ProductionWithUft != null)
									{
										if (bill_ProductionWithUft.BoundUft != null)
										{
											if (bill_ProductionWithUft.BoundWorker != pawn || !pawn.CanReserveAndReach(bill_ProductionWithUft.BoundUft, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false) || bill_ProductionWithUft.BoundUft.IsForbidden(pawn))
											{
												goto IL_1DC;
											}
											return WorkGiver_DoBill.FinishUftJob(pawn, bill_ProductionWithUft.BoundUft, bill_ProductionWithUft);
										}
										else
										{
											UnfinishedThing unfinishedThing = WorkGiver_DoBill.ClosestUnfinishedThingForBill(pawn, bill_ProductionWithUft);
											if (unfinishedThing != null)
											{
												return WorkGiver_DoBill.FinishUftJob(pawn, unfinishedThing, bill_ProductionWithUft);
											}
										}
									}
									if (!WorkGiver_DoBill.TryFindBestBillIngredients(bill, pawn, (Thing)giver, this.chosenIngThings))
									{
										if (FloatMenuMakerMap.makingFor != pawn)
										{
											bill.lastIngredientSearchFailTicks = Find.TickManager.TicksGame;
										}
										else
										{
											JobFailReason.Is(WorkGiver_DoBill.MissingMaterialsTranslated, bill.Label);
										}
										goto IL_1DC;
									}
									return this.TryStartNewDoBillJob(pawn, bill, giver);
								}
								JobFailReason.Is("UnderRequiredSkill".Translate(new object[]
								{
									skillRequirement.minLevel
								}), bill.Label);
							}
						}
					}
				}
				IL_1DC:;
			}
			return null;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00043900 File Offset: 0x00041D00
		private Job TryStartNewDoBillJob(Pawn pawn, Bill bill, IBillGiver giver)
		{
			Job job = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, giver, null);
			Job result;
			if (job != null)
			{
				result = job;
			}
			else
			{
				Job job2 = new Job(JobDefOf.DoBill, (Thing)giver);
				job2.targetQueueB = new List<LocalTargetInfo>(this.chosenIngThings.Count);
				job2.countQueue = new List<int>(this.chosenIngThings.Count);
				for (int i = 0; i < this.chosenIngThings.Count; i++)
				{
					job2.targetQueueB.Add(this.chosenIngThings[i].Thing);
					job2.countQueue.Add(this.chosenIngThings[i].Count);
				}
				job2.haulMode = HaulMode.ToCellNonStorage;
				job2.bill = bill;
				result = job2;
			}
			return result;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000439E0 File Offset: 0x00041DE0
		public bool ThingIsUsableBillGiver(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			Corpse corpse = thing as Corpse;
			Pawn pawn2 = null;
			if (corpse != null)
			{
				pawn2 = corpse.InnerPawn;
			}
			bool result;
			if (this.def.fixedBillGiverDefs != null && this.def.fixedBillGiverDefs.Contains(thing.def))
			{
				result = true;
			}
			else
			{
				if (pawn != null)
				{
					if (this.def.billGiversAllHumanlikes && pawn.RaceProps.Humanlike)
					{
						return true;
					}
					if (this.def.billGiversAllMechanoids && pawn.RaceProps.IsMechanoid)
					{
						return true;
					}
					if (this.def.billGiversAllAnimals && pawn.RaceProps.Animal)
					{
						return true;
					}
				}
				if (corpse != null && pawn2 != null)
				{
					if (this.def.billGiversAllHumanlikesCorpses && pawn2.RaceProps.Humanlike)
					{
						return true;
					}
					if (this.def.billGiversAllMechanoidsCorpses && pawn2.RaceProps.IsMechanoid)
					{
						return true;
					}
					if (this.def.billGiversAllAnimalsCorpses && pawn2.RaceProps.Animal)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00043B48 File Offset: 0x00041F48
		private static bool TryFindBestBillIngredients(Bill bill, Pawn pawn, Thing billGiver, List<ThingCount> chosen)
		{
			chosen.Clear();
			WorkGiver_DoBill.newRelevantThings.Clear();
			bool result;
			if (bill.recipe.ingredients.Count == 0)
			{
				result = true;
			}
			else
			{
				IntVec3 rootCell = WorkGiver_DoBill.GetBillGiverRootCell(billGiver, pawn);
				Region rootReg = rootCell.GetRegion(pawn.Map, RegionType.Set_Passable);
				if (rootReg == null)
				{
					result = false;
				}
				else
				{
					WorkGiver_DoBill.MakeIngredientsListInProcessingOrder(WorkGiver_DoBill.ingredientsOrdered, bill);
					WorkGiver_DoBill.relevantThings.Clear();
					WorkGiver_DoBill.processedThings.Clear();
					bool foundAll = false;
					Predicate<Thing> baseValidator = (Thing t) => t.Spawned && !t.IsForbidden(pawn) && (float)(t.Position - billGiver.Position).LengthHorizontalSquared < bill.ingredientSearchRadius * bill.ingredientSearchRadius && bill.IsFixedOrAllowedIngredient(t) && bill.recipe.ingredients.Any((IngredientCount ingNeed) => ingNeed.filter.Allows(t)) && pawn.CanReserve(t, 1, -1, null, false);
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
					RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParams, false);
					int adjacentRegionsAvailable = rootReg.Neighbors.Count((Region region) => entryCondition(rootReg, region));
					int regionsProcessed = 0;
					WorkGiver_DoBill.processedThings.AddRange(WorkGiver_DoBill.relevantThings);
					RegionProcessor regionProcessor = delegate(Region r)
					{
						List<Thing> list = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
						for (int i = 0; i < list.Count; i++)
						{
							Thing thing = list[i];
							if (!WorkGiver_DoBill.processedThings.Contains(thing))
							{
								if (ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, pawn))
								{
									if (baseValidator(thing) && (!thing.def.IsMedicine || !billGiverIsPawn))
									{
										WorkGiver_DoBill.newRelevantThings.Add(thing);
										WorkGiver_DoBill.processedThings.Add(thing);
									}
								}
							}
						}
						regionsProcessed++;
						if (WorkGiver_DoBill.newRelevantThings.Count > 0 && regionsProcessed > adjacentRegionsAvailable)
						{
							Comparison<Thing> comparison = delegate(Thing t1, Thing t2)
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
								foundAll = true;
								return true;
							}
						}
						return false;
					};
					RegionTraverser.BreadthFirstTraverse(rootReg, entryCondition, regionProcessor, 99999, RegionType.Set_Passable);
					WorkGiver_DoBill.relevantThings.Clear();
					WorkGiver_DoBill.newRelevantThings.Clear();
					result = foundAll;
				}
			}
			return result;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00043D40 File Offset: 0x00042140
		private static IntVec3 GetBillGiverRootCell(Thing billGiver, Pawn forPawn)
		{
			Building building = billGiver as Building;
			IntVec3 result;
			if (building != null)
			{
				if (building.def.hasInteractionCell)
				{
					result = building.InteractionCell;
				}
				else
				{
					Log.Error("Tried to find bill ingredients for " + billGiver + " which has no interaction cell.", false);
					result = forPawn.Position;
				}
			}
			else
			{
				result = billGiver.Position;
			}
			return result;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00043DA8 File Offset: 0x000421A8
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
			WorkGiver_DoBill.tmpMedicine.SortBy((Thing x) => -x.GetStatValue(StatDefOf.MedicalPotency, true), (Thing x) => x.Position.DistanceToSquared(billGiver.Position));
			relevantThings.AddRange(WorkGiver_DoBill.tmpMedicine);
			WorkGiver_DoBill.tmpMedicine.Clear();
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00043E94 File Offset: 0x00042294
		private static MedicalCareCategory GetMedicalCareCategory(Thing billGiver)
		{
			Pawn pawn = billGiver as Pawn;
			MedicalCareCategory result;
			if (pawn != null && pawn.playerSettings != null)
			{
				result = pawn.playerSettings.medCare;
			}
			else
			{
				result = MedicalCareCategory.Best;
			}
			return result;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00043ED4 File Offset: 0x000422D4
		private static void MakeIngredientsListInProcessingOrder(List<IngredientCount> ingredientsOrdered, Bill bill)
		{
			ingredientsOrdered.Clear();
			if (bill.recipe.productHasIngredientStuff)
			{
				ingredientsOrdered.Add(bill.recipe.ingredients[0]);
			}
			for (int i = 0; i < bill.recipe.ingredients.Count; i++)
			{
				if (!bill.recipe.productHasIngredientStuff || i != 0)
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

		// Token: 0x0600068E RID: 1678 RVA: 0x00043FBC File Offset: 0x000423BC
		private static bool TryFindBestBillIngredientsInSet(List<Thing> availableThings, Bill bill, List<ThingCount> chosen)
		{
			bool result;
			if (bill.recipe.allowMixingIngredients)
			{
				result = WorkGiver_DoBill.TryFindBestBillIngredientsInSet_AllowMix(availableThings, bill, chosen);
			}
			else
			{
				result = WorkGiver_DoBill.TryFindBestBillIngredientsInSet_NoMix(availableThings, bill, chosen);
			}
			return result;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00043FF8 File Offset: 0x000423F8
		private static bool TryFindBestBillIngredientsInSet_NoMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen)
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
					if (num <= WorkGiver_DoBill.availableCounts.GetCount(j))
					{
						if (ingredientCount.filter.Allows(WorkGiver_DoBill.availableCounts.GetDef(j)))
						{
							if (ingredientCount.IsFixedIngredient || bill.ingredientFilter.Allows(WorkGiver_DoBill.availableCounts.GetDef(j)))
							{
								for (int k = 0; k < availableThings.Count; k++)
								{
									if (availableThings[k].def == WorkGiver_DoBill.availableCounts.GetDef(j))
									{
										int num2 = availableThings[k].stackCount - ThingCountUtility.CountOf(chosen, availableThings[k]);
										if (num2 > 0)
										{
											int num3 = Mathf.Min(Mathf.FloorToInt(num), num2);
											ThingCountUtility.AddToList(chosen, availableThings[k], num3);
											num -= (float)num3;
											if (num < 0.001f)
											{
												flag = true;
												float num4 = WorkGiver_DoBill.availableCounts.GetCount(j);
												num4 -= (float)ingredientCount.CountRequiredOfFor(WorkGiver_DoBill.availableCounts.GetDef(j), bill.recipe);
												WorkGiver_DoBill.availableCounts.SetCount(j, num4);
												break;
											}
										}
									}
								}
								if (flag)
								{
									break;
								}
							}
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000441FC File Offset: 0x000425FC
		private static bool TryFindBestBillIngredientsInSet_AllowMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen)
		{
			chosen.Clear();
			for (int i = 0; i < bill.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = bill.recipe.ingredients[i];
				float num = ingredientCount.GetBaseCount();
				for (int j = 0; j < availableThings.Count; j++)
				{
					Thing thing = availableThings[j];
					if (ingredientCount.filter.Allows(thing))
					{
						if (ingredientCount.IsFixedIngredient || bill.ingredientFilter.Allows(thing))
						{
							float num2 = bill.recipe.IngredientValueGetter.ValuePerUnitOf(thing.def);
							int num3 = Mathf.Min(Mathf.CeilToInt(num / num2), thing.stackCount);
							ThingCountUtility.AddToList(chosen, thing, num3);
							num -= (float)num3 * num2;
							if (num <= 0.0001f)
							{
								break;
							}
						}
					}
				}
				if (num > 0.0001f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0200013F RID: 319
		private class DefCountList
		{
			// Token: 0x04000321 RID: 801
			private List<ThingDef> defs = new List<ThingDef>();

			// Token: 0x04000322 RID: 802
			private List<float> counts = new List<float>();

			// Token: 0x170000FC RID: 252
			// (get) Token: 0x06000694 RID: 1684 RVA: 0x000443B8 File Offset: 0x000427B8
			public int Count
			{
				get
				{
					return this.defs.Count;
				}
			}

			// Token: 0x170000FD RID: 253
			public float this[ThingDef def]
			{
				get
				{
					int num = this.defs.IndexOf(def);
					float result;
					if (num < 0)
					{
						result = 0f;
					}
					else
					{
						result = this.counts[num];
					}
					return result;
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

			// Token: 0x06000697 RID: 1687 RVA: 0x0004447C File Offset: 0x0004287C
			public float GetCount(int index)
			{
				return this.counts[index];
			}

			// Token: 0x06000698 RID: 1688 RVA: 0x0004449D File Offset: 0x0004289D
			public void SetCount(int index, float val)
			{
				this.counts[index] = val;
				this.CheckRemove(index);
			}

			// Token: 0x06000699 RID: 1689 RVA: 0x000444B4 File Offset: 0x000428B4
			public ThingDef GetDef(int index)
			{
				return this.defs[index];
			}

			// Token: 0x0600069A RID: 1690 RVA: 0x000444D5 File Offset: 0x000428D5
			private void CheckRemove(int index)
			{
				if (this.counts[index] == 0f)
				{
					this.counts.RemoveAt(index);
					this.defs.RemoveAt(index);
				}
			}

			// Token: 0x0600069B RID: 1691 RVA: 0x00044508 File Offset: 0x00042908
			public void Clear()
			{
				this.defs.Clear();
				this.counts.Clear();
			}

			// Token: 0x0600069C RID: 1692 RVA: 0x00044524 File Offset: 0x00042924
			public void GenerateFrom(List<Thing> things)
			{
				this.Clear();
				for (int i = 0; i < things.Count; i++)
				{
					ThingDef def;
					this[def = things[i].def] = this[def] + (float)things[i].stackCount;
				}
			}
		}
	}
}
