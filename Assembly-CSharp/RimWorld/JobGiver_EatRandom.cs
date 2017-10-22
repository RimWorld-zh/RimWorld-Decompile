using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_EatRandom : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing t)
			{
				if (t.def.category != ThingCategory.Item)
				{
					return false;
				}
				if (!t.IngestibleNow)
				{
					return false;
				}
				if (!pawn.RaceProps.CanEverEat(t))
				{
					return false;
				}
				return true;
			};
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 10f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing == null)
			{
				return null;
			}
			return new Job(JobDefOf.Ingest, thing);
		}
	}
}
