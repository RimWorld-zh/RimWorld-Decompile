using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobGiver_ManTurrets : ThinkNode_JobGiver
	{
		public float maxDistFromPoint = -1f;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ManTurrets jobGiver_ManTurrets = (JobGiver_ManTurrets)base.DeepCopy(resolve);
			jobGiver_ManTurrets.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_ManTurrets;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = (Predicate<Thing>)((Thing t) => (byte)(t.def.hasInteractionCell ? (t.def.HasComp(typeof(CompMannable)) ? (pawn.CanReserve(t, 1, -1, null, false) ? ((JobDriver_ManTurret.FindAmmoForTurret(pawn, (Building_TurretGun)t) != null) ? 1 : 0) : 0) : 0) : 0) != 0);
			Thing thing = GenClosest.ClosestThingReachable(this.GetRoot(pawn), pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), this.maxDistFromPoint, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			Job result;
			if (thing != null)
			{
				Job job = new Job(JobDefOf.ManTurret, thing);
				job.expiryInterval = 2000;
				job.checkOverrideOnExpire = true;
				result = job;
			}
			else
			{
				result = null;
			}
			return result;
		}

		protected abstract IntVec3 GetRoot(Pawn pawn);
	}
}
