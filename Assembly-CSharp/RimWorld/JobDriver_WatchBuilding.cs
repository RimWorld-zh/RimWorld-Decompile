using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_WatchBuilding : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			yield return Toils_Reserve.Reserve(TargetIndex.A, base.CurJob.def.joyMaxParticipants, 0, null);
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			Toil watch;
			if (base.TargetC.HasThing && base.TargetC.Thing is Building_Bed)
			{
				this.KeepLyingDown(TargetIndex.C);
				yield return Toils_Reserve.Reserve(TargetIndex.C, ((Building_Bed)base.TargetC.Thing).SleepingSlotsCount, 0, null);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.C, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.C);
				watch = Toils_LayDown.LayDown(TargetIndex.C, true, false, true, true);
				watch.AddFailCondition((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator1D)/*Error near IL_0166: stateMachine*/)._003Cwatch_003E__1.actor.Awake()));
			}
			else
			{
				if (base.TargetC.HasThing)
				{
					yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
				}
				yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				watch = new Toil();
			}
			watch.AddPreTickAction((Action)delegate
			{
				((_003CMakeNewToils_003Ec__Iterator1D)/*Error near IL_01da: stateMachine*/)._003C_003Ef__this.WatchTickAction();
			});
			watch.AddFinishAction((Action)delegate
			{
				JoyUtility.TryGainRecRoomThought(((_003CMakeNewToils_003Ec__Iterator1D)/*Error near IL_01f1: stateMachine*/)._003C_003Ef__this.pawn);
			});
			watch.defaultCompleteMode = ToilCompleteMode.Delay;
			watch.defaultDuration = base.CurJob.def.joyDuration;
			yield return watch;
		}

		protected virtual void WatchTickAction()
		{
			base.pawn.Drawer.rotator.FaceCell(base.TargetA.Cell);
			base.pawn.GainComfortFromCellIfPossible();
			float statValue;
			float num = statValue = base.TargetThingA.GetStatValue(StatDefOf.EntertainmentStrengthFactor, true);
			JoyUtility.JoyTickCheckEnd(base.pawn, JoyTickFullJoyAction.EndJob, statValue);
		}
	}
}
