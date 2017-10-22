using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_FleeAndCower : JobDriver_Flee
	{
		private const int CowerTicks = 1200;

		private const int CheckFleeAgainIntervalTicks = 35;

		public override string GetReport()
		{
			return (base.pawn.CurJob == base.job && !(base.pawn.Position != base.job.GetTarget(TargetIndex.A).Cell)) ? "ReportCowering".Translate() : base.GetReport();
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			using (IEnumerator<Toil> enumerator = this._003CMakeNewToils_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Toil toil = enumerator.Current;
					yield return toil;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 1200,
				tickAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00e6: stateMachine*/)._0024this.pawn.IsHashIntervalTick(35) && SelfDefenseUtility.ShouldStartFleeing(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00e6: stateMachine*/)._0024this.pawn))
					{
						((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00e6: stateMachine*/)._0024this.EndJobWith(JobCondition.InterruptForced);
					}
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0121:
			/*Error near IL_0122: Unexpected return in MoveNext()*/;
		}
	}
}
