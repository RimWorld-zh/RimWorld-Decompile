using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200006A RID: 106
	public class JobDriver_FleeAndCower : JobDriver_Flee
	{
		// Token: 0x060002EC RID: 748 RVA: 0x0001F8E8 File Offset: 0x0001DCE8
		public override string GetReport()
		{
			string result;
			if (this.pawn.CurJob != this.job || this.pawn.Position != this.job.GetTarget(TargetIndex.A).Cell)
			{
				result = base.GetReport();
			}
			else
			{
				result = "ReportCowering".Translate();
			}
			return result;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0001F954 File Offset: 0x0001DD54
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in this.<MakeNewToils>__BaseCallProxy0())
			{
				yield return toil;
			}
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 1200,
				tickAction = delegate()
				{
					if (this.pawn.IsHashIntervalTick(35) && SelfDefenseUtility.ShouldStartFleeing(this.pawn))
					{
						base.EndJobWith(JobCondition.InterruptForced);
					}
				}
			};
			yield break;
		}

		// Token: 0x0400020D RID: 525
		private const int CowerTicks = 1200;

		// Token: 0x0400020E RID: 526
		private const int CheckFleeAgainIntervalTicks = 35;
	}
}
