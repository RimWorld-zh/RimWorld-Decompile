using System;

namespace Verse.AI
{
	// Token: 0x02000A54 RID: 2644
	public static class Toils_ReserveAttackTarget
	{
		// Token: 0x06003ADB RID: 15067 RVA: 0x001F3D7C File Offset: 0x001F217C
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
