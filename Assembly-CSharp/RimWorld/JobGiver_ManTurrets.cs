using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobGiver_ManTurrets : ThinkNode_JobGiver
	{
		public float maxDistFromPoint = -1f;

		protected JobGiver_ManTurrets()
		{
		}

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ManTurrets jobGiver_ManTurrets = (JobGiver_ManTurrets)base.DeepCopy(resolve);
			jobGiver_ManTurrets.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_ManTurrets;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = (Thing t) => t.def.hasInteractionCell && t.def.HasComp(typeof(CompMannable)) && pawn.CanReserve(t, 1, -1, null, false) && JobDriver_ManTurret.FindAmmoForTurret(pawn, (Building_TurretGun)t) != null;
			Thing thing = GenClosest.ClosestThingReachable(this.GetRoot(pawn), pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), this.maxDistFromPoint, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				return new Job(JobDefOf.ManTurret, thing)
				{
					expiryInterval = 2000,
					checkOverrideOnExpire = true
				};
			}
			return null;
		}

		protected abstract IntVec3 GetRoot(Pawn pawn);

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal Pawn pawn;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				return t.def.hasInteractionCell && t.def.HasComp(typeof(CompMannable)) && this.pawn.CanReserve(t, 1, -1, null, false) && JobDriver_ManTurret.FindAmmoForTurret(this.pawn, (Building_TurretGun)t) != null;
			}
		}
	}
}
