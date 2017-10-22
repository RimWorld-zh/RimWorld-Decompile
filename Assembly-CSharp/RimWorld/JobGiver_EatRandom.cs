using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_EatRandom : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.Downed)
			{
				result = null;
			}
			else
			{
				Predicate<Thing> validator = (Predicate<Thing>)((Thing t) => (byte)((t.def.category == ThingCategory.Item) ? (t.IngestibleNow ? (pawn.RaceProps.CanEverEat(t) ? (pawn.CanReserve(t, 1, -1, null, false) ? 1 : 0) : 0) : 0) : 0) != 0);
				Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 10f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				result = ((thing != null) ? new Job(JobDefOf.Ingest, thing) : null);
			}
			return result;
		}
	}
}
