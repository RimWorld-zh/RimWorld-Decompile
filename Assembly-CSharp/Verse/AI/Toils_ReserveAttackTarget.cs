using System;

namespace Verse.AI
{
	// Token: 0x02000A50 RID: 2640
	public static class Toils_ReserveAttackTarget
	{
		// Token: 0x06003AD6 RID: 15062 RVA: 0x001F4074 File Offset: 0x001F2474
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
	}
}
