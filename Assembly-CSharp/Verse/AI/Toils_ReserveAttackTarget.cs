using System;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	public static class Toils_ReserveAttackTarget
	{
		public static Toil TryReserve(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				IAttackTarget attackTarget = actor.CurJob.GetTarget(ind).Thing as IAttackTarget;
				if (attackTarget != null)
				{
					actor.Map.attackTargetReservationManager.Reserve(actor, toil.actor.CurJob, attackTarget);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		[CompilerGenerated]
		private sealed class <TryReserve>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <TryReserve>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				IAttackTarget attackTarget = actor.CurJob.GetTarget(this.ind).Thing as IAttackTarget;
				if (attackTarget != null)
				{
					actor.Map.attackTargetReservationManager.Reserve(actor, this.toil.actor.CurJob, attackTarget);
				}
			}
		}
	}
}
