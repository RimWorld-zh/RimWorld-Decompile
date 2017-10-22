using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Warden_Feed : WorkGiver_Warden
	{
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = (Pawn)t;
				Thing t2 = default(Thing);
				ThingDef def = default(ThingDef);
				if (!WardenFeedUtility.ShouldBeFed(pawn2))
				{
					result = null;
				}
				else if (pawn2.needs.food.CurLevelPercentage >= pawn2.needs.food.PercentageThreshHungry + 0.019999999552965164)
				{
					result = null;
				}
				else if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out t2, out def, false, true, false, false, false, false))
				{
					JobFailReason.Is("NoFood".Translate());
					result = null;
				}
				else
				{
					Job job = new Job(JobDefOf.FeedPatient, t2, (Thing)pawn2);
					job.count = FoodUtility.WillIngestStackCountOf(pawn2, def);
					result = job;
				}
			}
			return result;
		}
	}
}
