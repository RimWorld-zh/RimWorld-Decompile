using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000D6 RID: 214
	public abstract class JobGiver_ManTurrets : ThinkNode_JobGiver
	{
		// Token: 0x040002A8 RID: 680
		public float maxDistFromPoint = -1f;

		// Token: 0x060004C1 RID: 1217 RVA: 0x00035724 File Offset: 0x00033B24
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ManTurrets jobGiver_ManTurrets = (JobGiver_ManTurrets)base.DeepCopy(resolve);
			jobGiver_ManTurrets.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_ManTurrets;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00035754 File Offset: 0x00033B54
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = (Thing t) => t.def.hasInteractionCell && t.def.HasComp(typeof(CompMannable)) && pawn.CanReserve(t, 1, -1, null, false) && JobDriver_ManTurret.FindAmmoForTurret(pawn, (Building_TurretGun)t) != null;
			Thing thing = GenClosest.ClosestThingReachable(this.GetRoot(pawn), pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), this.maxDistFromPoint, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			Job result;
			if (thing != null)
			{
				result = new Job(JobDefOf.ManTurret, thing)
				{
					expiryInterval = 2000,
					checkOverrideOnExpire = true
				};
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060004C3 RID: 1219
		protected abstract IntVec3 GetRoot(Pawn pawn);
	}
}
