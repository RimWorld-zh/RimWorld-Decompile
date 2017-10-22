using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_RescueNearby : ThinkNode_JobGiver
	{
		private float radius = 30f;

		private const float MinDistFromEnemy = 25f;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_RescueNearby jobGiver_RescueNearby = (JobGiver_RescueNearby)base.DeepCopy(resolve);
			jobGiver_RescueNearby.radius = this.radius;
			return jobGiver_RescueNearby;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing t)
			{
				Pawn pawn3 = (Pawn)t;
				return (byte)((pawn3.Downed && pawn3.Faction == pawn.Faction && !pawn3.InBed() && pawn.CanReserve((Thing)pawn3, 1, -1, null, false) && !pawn3.IsForbidden(pawn) && !GenAI.EnemyIsNear(pawn3, 25f)) ? 1 : 0) != 0;
			};
			Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), this.radius, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			Job result;
			if (pawn2 == null)
			{
				result = null;
			}
			else
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(pawn2, pawn, pawn2.HostFaction == pawn.Faction, false, false);
				if (building_Bed == null || !pawn2.CanReserve((Thing)building_Bed, 1, -1, null, false))
				{
					result = null;
				}
				else
				{
					Job job = new Job(JobDefOf.Rescue, (Thing)pawn2, (Thing)building_Bed);
					job.count = 1;
					result = job;
				}
			}
			return result;
		}
	}
}
