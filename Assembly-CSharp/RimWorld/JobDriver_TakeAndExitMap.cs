using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_TakeAndExitMap : JobDriver
	{
		private const TargetIndex ItemInd = TargetIndex.A;

		private const TargetIndex ExitCellInd = TargetIndex.B;

		protected Thing Item
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
			Toil gotoCell = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			gotoCell.AddPreTickAction((Action)delegate
			{
				if (((_003CMakeNewToils_003Ec__Iterator30)/*Error near IL_00c7: stateMachine*/)._003C_003Ef__this.Map.exitMapGrid.IsExitCell(((_003CMakeNewToils_003Ec__Iterator30)/*Error near IL_00c7: stateMachine*/)._003C_003Ef__this.pawn.Position))
				{
					((_003CMakeNewToils_003Ec__Iterator30)/*Error near IL_00c7: stateMachine*/)._003C_003Ef__this.pawn.ExitMap(true);
				}
			});
			yield return gotoCell;
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (!((_003CMakeNewToils_003Ec__Iterator30)/*Error near IL_0101: stateMachine*/)._003C_003Ef__this.pawn.Position.OnEdge(((_003CMakeNewToils_003Ec__Iterator30)/*Error near IL_0101: stateMachine*/)._003C_003Ef__this.pawn.Map) && !((_003CMakeNewToils_003Ec__Iterator30)/*Error near IL_0101: stateMachine*/)._003C_003Ef__this.pawn.Map.exitMapGrid.IsExitCell(((_003CMakeNewToils_003Ec__Iterator30)/*Error near IL_0101: stateMachine*/)._003C_003Ef__this.pawn.Position))
						return;
					((_003CMakeNewToils_003Ec__Iterator30)/*Error near IL_0101: stateMachine*/)._003C_003Ef__this.pawn.ExitMap(true);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
