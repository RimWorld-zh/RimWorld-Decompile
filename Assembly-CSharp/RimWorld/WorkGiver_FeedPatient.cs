using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_FeedPatient : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && pawn2 != pawn)
			{
				if (base.def.feedHumanlikesOnly && !pawn2.RaceProps.Humanlike)
				{
					return false;
				}
				if (base.def.feedAnimalsOnly && !pawn2.RaceProps.Animal)
				{
					return false;
				}
				if (pawn2.needs.food != null && !(pawn2.needs.food.CurLevelPercentage > pawn2.needs.food.PercentageThreshHungry + 0.019999999552965164))
				{
					if (!FeedPatientUtility.ShouldBeFed(pawn2))
					{
						return false;
					}
					if (!pawn.CanReserveAndReach(t, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, forced))
					{
						return false;
					}
					Thing thing = default(Thing);
					ThingDef thingDef = default(ThingDef);
					if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, true, false))
					{
						JobFailReason.Is("NoFood".Translate());
						return false;
					}
					return true;
				}
				return false;
			}
			return false;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Thing t2 = default(Thing);
			ThingDef def = default(ThingDef);
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out t2, out def, false, true, false, true, false))
			{
				Job job = new Job(JobDefOf.FeedPatient);
				job.targetA = t2;
				job.targetB = (Thing)pawn2;
				job.count = FoodUtility.WillIngestStackCountOf(pawn2, def);
				return job;
			}
			return null;
		}
	}
}
