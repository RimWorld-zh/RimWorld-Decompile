using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A4F RID: 2639
	public static class Toils_Reserve
	{
		// Token: 0x06003AD3 RID: 15059 RVA: 0x001F3D80 File Offset: 0x001F2180
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

		// Token: 0x06003AD4 RID: 15060 RVA: 0x001F3DF8 File Offset: 0x001F21F8
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

		// Token: 0x06003AD5 RID: 15061 RVA: 0x001F3E70 File Offset: 0x001F2270
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
