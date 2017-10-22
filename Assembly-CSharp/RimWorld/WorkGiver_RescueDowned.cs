using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_RescueDowned : WorkGiver_TakeToBed
	{
		private const float MinDistFromEnemy = 40f;

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && pawn2.Downed && pawn2.Faction == pawn.Faction && !pawn2.InBed() && pawn.CanReserve((Thing)pawn2, 1, -1, null, forced) && !GenAI.EnemyIsNear(pawn2, 40f))
			{
				Thing thing = base.FindBed(pawn, pawn2);
				if (thing != null && pawn2.CanReserve(thing, 1, -1, null, false))
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing t2 = base.FindBed(pawn, pawn2);
			Job job = new Job(JobDefOf.Rescue, (Thing)pawn2, t2);
			job.count = 1;
			return job;
		}
	}
}
