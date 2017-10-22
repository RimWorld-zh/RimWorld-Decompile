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
			Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing t)
			{
				if (!t.def.hasInteractionCell)
				{
					return false;
				}
				bool flag = false;
				int num = 0;
				while (num < t.def.comps.Count)
				{
					if (t.def.comps[num].compClass != typeof(CompMannable))
					{
						num++;
						continue;
					}
					flag = true;
					break;
				}
				if (!flag)
				{
					return false;
				}
				if (!pawn.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				if (JobDriver_ManTurret.FindAmmoForTurret(pawn, t) == null)
				{
					return false;
				}
				return true;
			};
			Thing thing = GenClosest.ClosestThingReachable(this.GetRoot(pawn), pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), this.maxDistFromPoint, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				Job job = new Job(JobDefOf.ManTurret, thing);
				job.expiryInterval = 2000;
				job.checkOverrideOnExpire = true;
				return job;
			}
			return null;
		}

		protected abstract IntVec3 GetRoot(Pawn pawn);
	}
}
