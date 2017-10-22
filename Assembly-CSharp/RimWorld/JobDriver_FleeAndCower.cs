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
			if (base.pawn.Position != base.CurJob.GetTarget(TargetIndex.A).Cell)
			{
				return base.GetReport();
			}
			return "ReportCowering".Translate();
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil item in base.MakeNewToils())
			{
				yield return item;
			}
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 1200,
				tickAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator2C)/*Error near IL_00d7: stateMachine*/)._003C_003Ef__this.pawn.IsHashIntervalTick(35) && SelfDefenseUtility.ShouldStartFleeing(((_003CMakeNewToils_003Ec__Iterator2C)/*Error near IL_00d7: stateMachine*/)._003C_003Ef__this.pawn))
					{
						((_003CMakeNewToils_003Ec__Iterator2C)/*Error near IL_00d7: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.InterruptForced);
					}
				}
			};
		}
	}
}
