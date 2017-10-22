using RimWorld.Planet;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_Goto : JobDriver
	{
		public override bool TryMakePreToilReservations()
		{
			base.pawn.Map.pawnDestinationReservationManager.Reserve(base.pawn, base.job, base.job.targetA.Cell);
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil gotoCell = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			gotoCell.AddPreTickAction((Action)delegate
			{
				if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003a: stateMachine*/)._0024this.job.exitMapOnArrival && ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003a: stateMachine*/)._0024this.pawn.Map.exitMapGrid.IsExitCell(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003a: stateMachine*/)._0024this.pawn.Position))
				{
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003a: stateMachine*/)._0024this.TryExitMap();
				}
			});
			gotoCell.FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0051: stateMachine*/)._0024this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0051: stateMachine*/)._0024this.pawn)));
			yield return gotoCell;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private void TryExitMap()
		{
			if (base.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(base.pawn))
				return;
			base.pawn.ExitMap(true);
		}
	}
}
