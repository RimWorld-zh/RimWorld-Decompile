using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Warden_Feed : WorkGiver_Warden
	{
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (!WardenFeedUtility.ShouldBeFed(pawn2))
			{
				return null;
			}
			if (pawn2.needs.food.CurLevelPercentage >= pawn2.needs.food.PercentageThreshHungry + 0.019999999552965164)
			{
				return null;
			}
			Thing t2 = default(Thing);
			ThingDef def = default(ThingDef);
			if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out t2, out def, false, true, false, false, false))
			{
				JobFailReason.Is("NoFood".Translate());
				return null;
			}
			Job job = new Job(JobDefOf.FeedPatient, t2, (Thing)pawn2);
			job.count = FoodUtility.WillIngestStackCountOf(pawn2, def);
			return job;
		}
	}
}
