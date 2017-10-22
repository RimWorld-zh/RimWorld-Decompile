using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_CleanFilth : JobDriver
	{
		private const TargetIndex FilthInd = TargetIndex.A;

		private float cleaningWorkDone;

		private float totalCleaningWorkDone;

		private float totalCleaningWorkRequired;

		private Filth Filth
		{
			get
			{
				return (Filth)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.ReserveQueue(TargetIndex.A, 1, -1, null);
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A);
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A);
			Toil checkNextQueuedTarget = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, checkNextQueuedTarget).JumpIfOutsideHomeArea(TargetIndex.A, checkNextQueuedTarget);
			Toil clean = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00de: stateMachine*/)._003C_003Ef__this.cleaningWorkDone = 0f;
					((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00de: stateMachine*/)._003C_003Ef__this.totalCleaningWorkDone = 0f;
					((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00de: stateMachine*/)._003C_003Ef__this.totalCleaningWorkRequired = ((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00de: stateMachine*/)._003C_003Ef__this.Filth.def.filth.cleaningWorkToReduceThickness * (float)((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00de: stateMachine*/)._003C_003Ef__this.Filth.thickness;
				},
				tickAction = (Action)delegate
				{
					Filth filth = ((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00f5: stateMachine*/)._003C_003Ef__this.Filth;
					((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00f5: stateMachine*/)._003C_003Ef__this.cleaningWorkDone += 1f;
					((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00f5: stateMachine*/)._003C_003Ef__this.totalCleaningWorkDone += 1f;
					if (((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00f5: stateMachine*/)._003C_003Ef__this.cleaningWorkDone > filth.def.filth.cleaningWorkToReduceThickness)
					{
						filth.ThinFilth();
						((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00f5: stateMachine*/)._003C_003Ef__this.cleaningWorkDone = 0f;
						if (filth.Destroyed)
						{
							((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00f5: stateMachine*/)._003Cclean_003E__2.actor.records.Increment(RecordDefOf.MessesCleaned);
							((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_00f5: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A);
			clean.WithProgressBar(TargetIndex.A, (Func<float>)(() => ((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_012b: stateMachine*/)._003C_003Ef__this.totalCleaningWorkDone / ((_003CMakeNewToils_003Ec__Iterator26)/*Error near IL_012b: stateMachine*/)._003C_003Ef__this.totalCleaningWorkRequired), true, -0.5f);
			clean.PlaySustainerOrSound((Func<SoundDef>)(() => SoundDefOf.Interact_CleanFilth));
			clean.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, checkNextQueuedTarget);
			clean.JumpIfOutsideHomeArea(TargetIndex.A, checkNextQueuedTarget);
			yield return clean;
			yield return checkNextQueuedTarget;
			yield return Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.A, initExtractTargetFromQueue);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.cleaningWorkDone, "cleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkDone, "totalCleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkRequired, "totalCleaningWorkRequired", 0f, false);
		}
	}
}
