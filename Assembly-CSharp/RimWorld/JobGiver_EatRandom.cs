using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000A4 RID: 164
	public class JobGiver_EatRandom : ThinkNode_JobGiver
	{
		// Token: 0x06000410 RID: 1040 RVA: 0x00030AD0 File Offset: 0x0002EED0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.Downed)
			{
				result = null;
			}
			else
			{
				Predicate<Thing> validator = (Thing t) => t.def.category == ThingCategory.Item && t.IngestibleNow && pawn.RaceProps.CanEverEat(t) && pawn.CanReserve(t, 1, -1, null, false);
				Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 10f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (thing == null)
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Ingest, thing);
				}
			}
			return result;
		}
	}
}
