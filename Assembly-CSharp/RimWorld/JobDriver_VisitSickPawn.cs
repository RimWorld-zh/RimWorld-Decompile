using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_VisitSickPawn : JobDriver
	{
		private const TargetIndex PatientInd = TargetIndex.A;

		private const TargetIndex ChairInd = TargetIndex.B;

		private Pawn Patient
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Thing Chair
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0048: stateMachine*/)._003C_003Ef__this.Patient.InBed() || !((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0048: stateMachine*/)._003C_003Ef__this.Patient.Awake()));
			if (this.Chair != null)
			{
				this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			}
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			if (this.Chair != null)
			{
				yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			}
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(base.pawn);
			yield return new Toil
			{
				tickAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.Patient.needs.joy.GainJoy((float)(((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.CurJob.def.joyGainRate * 0.00014400000509340316), ((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.CurJob.def.joyKind);
					if (((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.pawn.IsHashIntervalTick(320))
					{
						InteractionDef intDef = (!(Rand.Value < 0.800000011920929)) ? InteractionDefOf.DeepTalk : InteractionDefOf.Chitchat;
						((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.pawn.interactions.TryInteractWith(((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.Patient, intDef);
					}
					((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.pawn.Drawer.rotator.FaceCell(((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.Patient.Position);
					((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.pawn.GainComfortFromCellIfPossible();
					JoyUtility.JoyTickCheckEnd(((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.pawn, JoyTickFullJoyAction.None, 1f);
					if (((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.pawn.needs.joy.CurLevelPercentage > 0.99989998340606689 && ((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.Patient.needs.joy.CurLevelPercentage > 0.99989998340606689)
					{
						((_003CMakeNewToils_003Ec__Iterator23)/*Error near IL_0127: stateMachine*/)._003C_003Ef__this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
					}
				},
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = base.CurJob.def.joyDuration
			};
		}
	}
}
