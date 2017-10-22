using RimWorld.Planet;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_Goto : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil gotoCell = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			gotoCell.AddPreTickAction((Action)delegate
			{
				if (((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_0038: stateMachine*/)._003C_003Ef__this.CurJob.exitMapOnArrival && ((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_0038: stateMachine*/)._003C_003Ef__this.pawn.Map.exitMapGrid.IsExitCell(((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_0038: stateMachine*/)._003C_003Ef__this.pawn.Position))
				{
					((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_0038: stateMachine*/)._003C_003Ef__this.TryExitMap();
				}
			});
			gotoCell.FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_004f: stateMachine*/)._003C_003Ef__this.CurJob.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_004f: stateMachine*/)._003C_003Ef__this.pawn)));
			yield return gotoCell;
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.pawn.mindState != null && ((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.pawn.mindState.forcedGotoPosition == ((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.TargetA.Cell)
					{
						((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
					}
					if (((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.CurJob.exitMapOnArrival)
					{
						if (!((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.pawn.Position.OnEdge(((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.pawn.Map) && !((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.pawn.Map.exitMapGrid.IsExitCell(((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.pawn.Position))
							return;
						((_003CMakeNewToils_003Ec__Iterator1B2)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.TryExitMap();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}

		private void TryExitMap()
		{
			if (base.CurJob.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(base.pawn))
				return;
			base.pawn.ExitMap(true);
		}
	}
}
