using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_SitFacingBuilding : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			this.EndOnDespawnedOrNull(TargetIndex.B, JobCondition.Incompletable);
			yield return Toils_Reserve.Reserve(TargetIndex.A, base.CurJob.def.joyMaxParticipants, 0, null);
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
			Toil play = new Toil
			{
				tickAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator1F)/*Error near IL_00bd: stateMachine*/)._003C_003Ef__this.pawn.Drawer.rotator.FaceCell(((_003CMakeNewToils_003Ec__Iterator1F)/*Error near IL_00bd: stateMachine*/)._003C_003Ef__this.TargetA.Cell);
					((_003CMakeNewToils_003Ec__Iterator1F)/*Error near IL_00bd: stateMachine*/)._003C_003Ef__this.pawn.GainComfortFromCellIfPossible();
					float statValue;
					float num = statValue = ((_003CMakeNewToils_003Ec__Iterator1F)/*Error near IL_00bd: stateMachine*/)._003C_003Ef__this.TargetThingA.GetStatValue(StatDefOf.EntertainmentStrengthFactor, true);
					JoyUtility.JoyTickCheckEnd(((_003CMakeNewToils_003Ec__Iterator1F)/*Error near IL_00bd: stateMachine*/)._003C_003Ef__this.pawn, JoyTickFullJoyAction.EndJob, statValue);
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = base.CurJob.def.joyDuration
			};
			play.AddFinishAction((Action)delegate
			{
				JoyUtility.TryGainRecRoomThought(((_003CMakeNewToils_003Ec__Iterator1F)/*Error near IL_0100: stateMachine*/)._003C_003Ef__this.pawn);
			});
			yield return play;
		}
	}
}
