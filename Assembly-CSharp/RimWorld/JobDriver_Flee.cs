using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Flee : JobDriver
	{
		protected const TargetIndex DestInd = TargetIndex.A;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				atomicWithPrevious = true,
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator2B)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.Map.pawnDestinationManager.ReserveDestinationFor(((_003CMakeNewToils_003Ec__Iterator2B)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.pawn, ((_003CMakeNewToils_003Ec__Iterator2B)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.CurJob.GetTarget(TargetIndex.A).Cell);
					if (((_003CMakeNewToils_003Ec__Iterator2B)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.pawn.IsColonist)
					{
						MoteMaker.MakeColonistActionOverlay(((_003CMakeNewToils_003Ec__Iterator2B)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.pawn, ThingDefOf.Mote_ColonistFleeing);
					}
				}
			};
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
		}
	}
}
