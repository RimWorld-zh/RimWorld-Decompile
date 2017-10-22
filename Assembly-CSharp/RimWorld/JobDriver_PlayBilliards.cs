using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public class JobDriver_PlayBilliards : JobDriver
	{
		private const int ShotDuration = 600;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			yield return Toils_Reserve.Reserve(TargetIndex.A, base.CurJob.def.joyMaxParticipants, 0, null);
			Toil chooseCell = new Toil
			{
				initAction = (Action)delegate
				{
					int num2 = 0;
					while (true)
					{
						((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_0087: stateMachine*/)._003C_003Ef__this.CurJob.targetB = ((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_0087: stateMachine*/)._003C_003Ef__this.CurJob.targetA.Thing.RandomAdjacentCell8Way();
						num2++;
						if (num2 <= 100)
						{
							if (((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_0087: stateMachine*/)._003C_003Ef__this.pawn.CanReserve((IntVec3)((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_0087: stateMachine*/)._003C_003Ef__this.CurJob.targetB, 1, -1, null, false))
								return;
							continue;
						}
						break;
					}
					Log.Error(((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_0087: stateMachine*/)._003C_003Ef__this.pawn + " could not find cell adjacent to billiards table " + ((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_0087: stateMachine*/)._003C_003Ef__this.TargetThingA);
					((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_0087: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Errored);
				}
			};
			yield return chooseCell;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil play = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_00f5: stateMachine*/)._003C_003Ef__this.CurJob.locomotionUrgency = LocomotionUrgency.Walk;
				},
				tickAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.pawn.Drawer.rotator.FaceCell(((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.TargetA.Thing.OccupiedRect().ClosestCellTo(((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.pawn.Position));
					if (((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.pawn.jobs.curDriver.ticksLeftThisToil == 300)
					{
						SoundDefOf.PlayBilliards.PlayOneShot(new TargetInfo(((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.pawn.Position, ((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.pawn.Map, false));
					}
					if (Find.TickManager.TicksGame > ((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.startTick + ((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.CurJob.def.joyDuration)
					{
						((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						float statValue;
						float num = statValue = ((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.TargetThingA.GetStatValue(StatDefOf.EntertainmentStrengthFactor, true);
						JoyUtility.JoyTickCheckEnd(((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_010c: stateMachine*/)._003C_003Ef__this.pawn, JoyTickFullJoyAction.EndJob, statValue);
					}
				},
				socialMode = RandomSocialMode.SuperActive,
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 600
			};
			play.AddFinishAction((Action)delegate
			{
				JoyUtility.TryGainRecRoomThought(((_003CMakeNewToils_003Ec__Iterator1C)/*Error near IL_014b: stateMachine*/)._003C_003Ef__this.pawn);
			});
			yield return play;
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield return Toils_Jump.Jump(chooseCell);
		}
	}
}
