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
			if (base.pawn.CurJob == base.job && !(base.pawn.Position != base.job.GetTarget(TargetIndex.A).Cell))
			{
				return "ReportCowering".Translate();
			}
			return base.GetReport();
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			using (IEnumerator<Toil> enumerator = base.MakeNewToils().GetEnumerator())
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
				tickAction = delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00e1: stateMachine*/)._0024this.pawn.IsHashIntervalTick(35) && SelfDefenseUtility.ShouldStartFleeing(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00e1: stateMachine*/)._0024this.pawn))
					{
						((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00e1: stateMachine*/)._0024this.EndJobWith(JobCondition.InterruptForced);
					}
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_011b:
			/*Error near IL_011c: Unexpected return in MoveNext()*/;
		}
	}
}
