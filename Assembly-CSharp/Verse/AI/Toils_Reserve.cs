using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	public static class Toils_Reserve
	{
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

		[CompilerGenerated]
		private sealed class <Reserve>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal int maxPawns;

			internal int stackCount;

			internal ReservationLayerDef layer;

			public <Reserve>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				if (!this.toil.actor.Reserve(this.toil.actor.jobs.curJob.GetTarget(this.ind), this.toil.actor.CurJob, this.maxPawns, this.stackCount, this.layer))
				{
					this.toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ReserveQueue>c__AnonStorey1
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal int maxPawns;

			internal int stackCount;

			internal ReservationLayerDef layer;

			public <ReserveQueue>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				List<LocalTargetInfo> targetQueue = this.toil.actor.jobs.curJob.GetTargetQueue(this.ind);
				if (targetQueue != null)
				{
					for (int i = 0; i < targetQueue.Count; i++)
					{
						if (!this.toil.actor.Reserve(targetQueue[i], this.toil.actor.CurJob, this.maxPawns, this.stackCount, this.layer))
						{
							this.toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <Release>c__AnonStorey2
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <Release>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				this.toil.actor.Map.reservationManager.Release(this.toil.actor.jobs.curJob.GetTarget(this.ind), this.toil.actor, this.toil.actor.CurJob);
			}
		}
	}
}
