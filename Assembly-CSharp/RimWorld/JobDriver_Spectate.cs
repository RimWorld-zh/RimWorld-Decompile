using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200004E RID: 78
	public class JobDriver_Spectate : JobDriver
	{
		// Token: 0x040001E1 RID: 481
		private const TargetIndex MySpotOrChairInd = TargetIndex.A;

		// Token: 0x040001E2 RID: 482
		private const TargetIndex WatchTargetInd = TargetIndex.B;

		// Token: 0x06000270 RID: 624 RVA: 0x00019CA0 File Offset: 0x000180A0
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00019CD8 File Offset: 0x000180D8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool haveChair = this.job.GetTarget(TargetIndex.A).HasThing;
			if (haveChair)
			{
				this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return new Toil
			{
				tickAction = delegate()
				{
					this.pawn.rotationTracker.FaceCell(this.job.GetTarget(TargetIndex.B).Cell);
					this.pawn.GainComfortFromCellIfPossible();
					if (this.pawn.IsHashIntervalTick(100))
					{
						this.pawn.jobs.CheckForJobOverride();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
			yield break;
		}
	}
}
