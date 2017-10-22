using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Spectate : JobDriver
	{
		private const TargetIndex MySpotOrChairInd = TargetIndex.A;

		private const TargetIndex WatchTargetInd = TargetIndex.B;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (base.CurJob.GetTarget(TargetIndex.A).HasThing)
			{
				this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			}
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return new Toil
			{
				tickAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator17)/*Error near IL_00a6: stateMachine*/)._003C_003Ef__this.pawn.Drawer.rotator.FaceCell(((_003CMakeNewToils_003Ec__Iterator17)/*Error near IL_00a6: stateMachine*/)._003C_003Ef__this.CurJob.GetTarget(TargetIndex.B).Cell);
					((_003CMakeNewToils_003Ec__Iterator17)/*Error near IL_00a6: stateMachine*/)._003C_003Ef__this.pawn.GainComfortFromCellIfPossible();
					if (((_003CMakeNewToils_003Ec__Iterator17)/*Error near IL_00a6: stateMachine*/)._003C_003Ef__this.pawn.IsHashIntervalTick(100))
					{
						((_003CMakeNewToils_003Ec__Iterator17)/*Error near IL_00a6: stateMachine*/)._003C_003Ef__this.pawn.jobs.CheckForJobOverride();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
		}
	}
}
