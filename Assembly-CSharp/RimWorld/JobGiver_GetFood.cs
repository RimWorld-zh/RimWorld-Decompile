using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000E1 RID: 225
	public class JobGiver_GetFood : ThinkNode_JobGiver
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x00036764 File Offset: 0x00034B64
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetFood jobGiver_GetFood = (JobGiver_GetFood)base.DeepCopy(resolve);
			jobGiver_GetFood.minCategory = this.minCategory;
			jobGiver_GetFood.forceScanWholeMap = this.forceScanWholeMap;
			return jobGiver_GetFood;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000367A0 File Offset: 0x00034BA0
		public override float GetPriority(Pawn pawn)
		{
			Need_Food food = pawn.needs.food;
			float result;
			if (food == null)
			{
				result = 0f;
			}
			else if (pawn.needs.food.CurCategory < HungerCategory.Starving && FoodUtility.ShouldBeFedBySomeone(pawn))
			{
				result = 0f;
			}
			else if (food.CurCategory < this.minCategory)
			{
				result = 0f;
			}
			else if (food.CurLevelPercentage < pawn.RaceProps.FoodLevelPercentageWantEat)
			{
				result = 9.5f;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00036840 File Offset: 0x00034C40
		protected override Job TryGiveJob(Pawn pawn)
		{
			Need_Food food = pawn.needs.food;
			Job result;
			if (food == null || food.CurCategory < this.minCategory)
			{
				result = null;
			}
			else
			{
				bool flag;
				if (pawn.AnimalOrWildMan())
				{
					flag = true;
				}
				else
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
					flag = (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.4f);
				}
				bool flag2 = pawn.needs.food.CurCategory == HungerCategory.Starving;
				bool desperate = flag2;
				Thing thing;
				ref Thing foodSource = ref thing;
				ThingDef thingDef;
				ref ThingDef foodDef = ref thingDef;
				bool canRefillDispenser = true;
				bool canUseInventory = true;
				bool allowCorpse = flag;
				bool flag3 = this.forceScanWholeMap;
				if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, desperate, out foodSource, out foodDef, canRefillDispenser, canUseInventory, false, allowCorpse, false, pawn.IsWildMan(), flag3))
				{
					result = null;
				}
				else
				{
					Pawn pawn2 = thing as Pawn;
					if (pawn2 != null)
					{
						result = new Job(JobDefOf.PredatorHunt, pawn2)
						{
							killIncappedTarget = true
						};
					}
					else if (thing is Plant && thing.def.plant.harvestedThingDef == thingDef)
					{
						result = new Job(JobDefOf.Harvest, thing);
					}
					else
					{
						Building_NutrientPasteDispenser building_NutrientPasteDispenser = thing as Building_NutrientPasteDispenser;
						if (building_NutrientPasteDispenser != null && !building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers())
						{
							Building building = building_NutrientPasteDispenser.AdjacentReachableHopper(pawn);
							if (building != null)
							{
								ISlotGroupParent hopperSgp = building as ISlotGroupParent;
								Job job = WorkGiver_CookFillHopper.HopperFillFoodJob(pawn, hopperSgp);
								if (job != null)
								{
									return job;
								}
							}
							thing = FoodUtility.BestFoodSourceOnMap(pawn, pawn, flag2, out thingDef, FoodPreferability.MealLavish, false, !pawn.IsTeetotaler(), false, false, false, false, false, false, this.forceScanWholeMap);
							if (thing == null)
							{
								return null;
							}
						}
						float nutrition = FoodUtility.GetNutrition(thing, thingDef);
						result = new Job(JobDefOf.Ingest, thing)
						{
							count = FoodUtility.WillIngestStackCountOf(pawn, thingDef, nutrition)
						};
					}
				}
			}
			return result;
		}

		// Token: 0x040002B6 RID: 694
		private HungerCategory minCategory = HungerCategory.Fed;

		// Token: 0x040002B7 RID: 695
		public bool forceScanWholeMap;
	}
}
