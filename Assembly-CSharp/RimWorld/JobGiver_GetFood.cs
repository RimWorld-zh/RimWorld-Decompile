using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_GetFood : ThinkNode_JobGiver
	{
		private HungerCategory minCategory;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetFood jobGiver_GetFood = (JobGiver_GetFood)base.DeepCopy(resolve);
			jobGiver_GetFood.minCategory = this.minCategory;
			return jobGiver_GetFood;
		}

		public override float GetPriority(Pawn pawn)
		{
			Need_Food food = pawn.needs.food;
			if (food == null)
			{
				return 0f;
			}
			if ((int)pawn.needs.food.CurCategory < 3 && FoodUtility.ShouldBeFedBySomeone(pawn))
			{
				return 0f;
			}
			if ((int)food.CurCategory < (int)this.minCategory)
			{
				return 0f;
			}
			if (food.CurLevelPercentage < pawn.RaceProps.FoodLevelPercentageWantEat)
			{
				return 9.5f;
			}
			return 0f;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Need_Food food = pawn.needs.food;
			if (food != null && (int)food.CurCategory >= (int)this.minCategory)
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
				bool flag2 = pawn.needs.food.CurCategory == HungerCategory.Starving;
				bool desperate = flag2;
				bool canRefillDispenser = true;
				bool canUseInventory = true;
				bool allowCorpse = flag;
				Thing thing = default(Thing);
				ThingDef thingDef = default(ThingDef);
				if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, desperate, out thing, out thingDef, canRefillDispenser, canUseInventory, false, allowCorpse, false, pawn.IsWildMan()))
				{
					return null;
				}
				Pawn pawn2 = thing as Pawn;
				if (pawn2 != null)
				{
					Job job = new Job(JobDefOf.PredatorHunt, pawn2);
					job.killIncappedTarget = true;
					return job;
				}
				if (thing is Plant && thing.def.plant.harvestedThingDef == thingDef)
				{
					return new Job(JobDefOf.Harvest, thing);
				}
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
							return job2;
						}
					}
					thing = FoodUtility.BestFoodSourceOnMap(pawn, pawn, flag2, out thingDef, FoodPreferability.MealLavish, false, !pawn.IsTeetotaler(), false, false, false, false, false, false);
					if (thing == null)
					{
						return null;
					}
				}
				Job job3 = new Job(JobDefOf.Ingest, thing);
				job3.count = FoodUtility.WillIngestStackCountOf(pawn, thingDef);
				return job3;
			}
			return null;
		}
	}
}
