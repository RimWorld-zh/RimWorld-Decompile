using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000ED RID: 237
	public class JobGiver_RescueNearby : ThinkNode_JobGiver
	{
		// Token: 0x040002CC RID: 716
		private float radius = 30f;

		// Token: 0x040002CD RID: 717
		private const float MinDistFromEnemy = 25f;

		// Token: 0x0600050D RID: 1293 RVA: 0x00038138 File Offset: 0x00036538
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_RescueNearby jobGiver_RescueNearby = (JobGiver_RescueNearby)base.DeepCopy(resolve);
			jobGiver_RescueNearby.radius = this.radius;
			return jobGiver_RescueNearby;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00038168 File Offset: 0x00036568
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				Pawn pawn3 = (Pawn)t;
				return pawn3.Downed && pawn3.Faction == pawn.Faction && !pawn3.InBed() && pawn.CanReserve(pawn3, 1, -1, null, false) && !pawn3.IsForbidden(pawn) && !GenAI.EnemyIsNear(pawn3, 25f);
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
				if (building_Bed == null || !pawn2.CanReserve(building_Bed, 1, -1, null, false))
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Rescue, pawn2, building_Bed)
					{
						count = 1
					};
				}
			}
			return result;
		}
	}
}
