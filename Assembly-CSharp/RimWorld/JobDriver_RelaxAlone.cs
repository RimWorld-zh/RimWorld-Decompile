using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_RelaxAlone : JobDriver
	{
		private Rot4 faceDir;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator1E)/*Error near IL_006e: stateMachine*/)._003C_003Ef__this.faceDir = ((!((_003CMakeNewToils_003Ec__Iterator1E)/*Error near IL_006e: stateMachine*/)._003C_003Ef__this.CurJob.def.faceDir.IsValid) ? Rot4.Random : ((_003CMakeNewToils_003Ec__Iterator1E)/*Error near IL_006e: stateMachine*/)._003C_003Ef__this.CurJob.def.faceDir);
				},
				tickAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator1E)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.pawn.Drawer.rotator.FaceCell(((_003CMakeNewToils_003Ec__Iterator1E)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.pawn.Position + ((_003CMakeNewToils_003Ec__Iterator1E)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.faceDir.FacingCell);
					((_003CMakeNewToils_003Ec__Iterator1E)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.pawn.GainComfortFromCellIfPossible();
					JoyUtility.JoyTickCheckEnd(((_003CMakeNewToils_003Ec__Iterator1E)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.pawn, JoyTickFullJoyAction.EndJob, 1f);
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = base.CurJob.def.joyDuration
			};
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.faceDir, "faceDir", default(Rot4), false);
		}
	}
}
