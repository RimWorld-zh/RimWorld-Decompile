using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A51 RID: 2641
	public static class Toils_Reserve
	{
		// Token: 0x06003AD7 RID: 15063 RVA: 0x001F3EAC File Offset: 0x001F22AC
		public static Toil Reserve(TargetIndex ind, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (!toil.actor.Reserve(toil.actor.jobs.curJob.GetTarget(ind), toil.actor.CurJob, maxPawns, stackCount, layer))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x001F3F24 File Offset: 0x001F2324
		public static Toil ReserveQueue(TargetIndex ind, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				List<LocalTargetInfo> targetQueue = toil.actor.jobs.curJob.GetTargetQueue(ind);
				if (targetQueue != null)
				{
					for (int i = 0; i < targetQueue.Count; i++)
					{
						if (!toil.actor.Reserve(targetQueue[i], toil.actor.CurJob, maxPawns, stackCount, layer))
						{
							toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
						}
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x001F3F9C File Offset: 0x001F239C
		public static Toil Release(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.Map.reservationManager.Release(toil.actor.jobs.curJob.GetTarget(ind), toil.actor, toil.actor.CurJob);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}
	}
}
