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
				return PathEndMode.ClosestTouch;
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool result;
			if (pawn2 == null || pawn2 == pawn)
			{
				result = false;
			}
			else if (base.def.feedHumanlikesOnly && !pawn2.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (base.def.feedAnimalsOnly && !pawn2.RaceProps.Animal)
			{
				result = false;
			}
			else if (pawn2.needs.food == null || pawn2.needs.food.CurLevelPercentage > pawn2.needs.food.PercentageThreshHungry + 0.019999999552965164)
			{
				result = false;
			}
			else if (!FeedPatientUtility.ShouldBeFed(pawn2))
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				Thing thing = default(Thing);
				ThingDef thingDef = default(ThingDef);
				if (!pawn.CanReserve(target, 1, -1, null, forced))
				{
					result = false;
				}
				else if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, true, false, false))
				{
					JobFailReason.Is("NoFood".Translate());
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Thing t2 = default(Thing);
			ThingDef def = default(ThingDef);
			Job result;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out t2, out def, false, true, false, true, false, false))
			{
				Job job = new Job(JobDefOf.FeedPatient);
				job.targetA = t2;
				job.targetB = (Thing)pawn2;
				job.count = FoodUtility.WillIngestStackCountOf(pawn2, def);
				result = job;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
