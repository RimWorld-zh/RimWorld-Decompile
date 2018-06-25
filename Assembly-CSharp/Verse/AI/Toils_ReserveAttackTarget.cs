using System;

namespace Verse.AI
{
	// Token: 0x02000A52 RID: 2642
	public static class Toils_ReserveAttackTarget
	{
		// Token: 0x06003ADA RID: 15066 RVA: 0x001F41A0 File Offset: 0x001F25A0
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
