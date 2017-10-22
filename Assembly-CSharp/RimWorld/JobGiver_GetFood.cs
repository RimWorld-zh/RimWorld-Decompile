using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_GetFood : ThinkNode_JobGiver
	{
		private HungerCategory minCategory = HungerCategory.Fed;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetFood jobGiver_GetFood = (JobGiver_GetFood)base.DeepCopy(resolve);
			jobGiver_GetFood.minCategory = this.minCategory;
			return jobGiver_GetFood;
		}

		public override float GetPriority(Pawn pawn)
		{
			Need_Food food = pawn.needs.food;
			return (float)((food != null) ? (((int)pawn.needs.food.CurCategory >= 3 || !FoodUtility.ShouldBeFedBySomeone(pawn)) ? (((int)food.CurCategory >= (int)this.minCategory) ? ((!(food.CurLevelPercentage < pawn.RaceProps.FoodLevelPercentageWantEat)) ? 0.0 : 9.5) : 0.0) : 0.0) : 0.0);
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Need_Food food = pawn.needs.food;
			Job result;
			if (food == null || (int)food.CurCategory < (int)this.minCategory)
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
					flag = (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.40000000596046448);
				}
				bool desperate;
				bool desperate2 = desperate = (pawn.needs.food.CurCategory == HungerCategory.Starving);
				bool canRefillDispenser = true;
				bool canUseInventory = true;
				bool allowCorpse = flag;
				Thing thing = default(Thing);
				ThingDef thingDef = default(ThingDef);
				if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, desperate, out thing, out thingDef, canRefillDispenser, canUseInventory, false, allowCorpse, false, pawn.IsWildMan()))
				{
					result = null;
				}
				else
				{
					Pawn pawn2 = thing as Pawn;
					if (pawn2 != null)
					{
						Job job = new Job(JobDefOf.PredatorHunt, (Thing)pawn2);
						job.killIncappedTarget = true;
						result = job;
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
								Job job2 = WorkGiver_CookFillHopper.HopperFillFoodJob(pawn, hopperSgp);
								if (job2 != null)
								{
									result = job2;
									goto IL_01e0;
								}
							}
							thing = FoodUtility.BestFoodSourceOnMap(pawn, pawn, desperate2, out thingDef, FoodPreferability.MealLavish, false, !pawn.IsTeetotaler(), false, false, false, false, false, false);
							if (thing == null)
							{
								result = null;
								goto IL_01e0;
							}
						}
						Job job3 = new Job(JobDefOf.Ingest, thing);
						job3.count = FoodUtility.WillIngestStackCountOf(pawn, thingDef);
						result = job3;
					}
				}
			}
			goto IL_01e0;
			IL_01e0:
			return result;
		}
	}
}
