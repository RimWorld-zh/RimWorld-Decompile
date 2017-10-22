using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_GatherAnimalBodyResources : JobDriver
	{
		protected const TargetIndex AnimalInd = TargetIndex.A;

		private float gatherProgress;

		protected abstract float WorkTotal
		{
			get;
		}

		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.gatherProgress, "gatherProgress", 0f, false);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil wait = new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor2 = ((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_0095: stateMachine*/)._003Cwait_003E__0.actor;
					Pawn pawn2 = (Pawn)((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_0095: stateMachine*/)._003Cwait_003E__0.actor.CurJob.GetTarget(TargetIndex.A).Thing;
					actor2.pather.StopDead();
					PawnUtility.ForceWait(pawn2, 15000, null, true);
				},
				tickAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_00ac: stateMachine*/)._003Cwait_003E__0.actor;
					actor.skills.Learn(SkillDefOf.Animals, 0.142999992f, false);
					((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_00ac: stateMachine*/)._003C_003Ef__this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true);
					if (((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_00ac: stateMachine*/)._003C_003Ef__this.gatherProgress >= ((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_00ac: stateMachine*/)._003C_003Ef__this.WorkTotal)
					{
						((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_00ac: stateMachine*/)._003C_003Ef__this.GetComp((Pawn)(Thing)((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_00ac: stateMachine*/)._003C_003Ef__this.CurJob.GetTarget(TargetIndex.A)).Gathered(((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_00ac: stateMachine*/)._003C_003Ef__this.pawn);
						actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
					}
				}
			};
			wait.AddFinishAction((Action)delegate
			{
				Pawn pawn = (Pawn)((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_00c3: stateMachine*/)._003Cwait_003E__0.actor.CurJob.GetTarget(TargetIndex.A).Thing;
				if (pawn.jobs.curJob.def == JobDefOf.WaitMaintainPosture)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				}
			});
			wait.FailOnDespawnedOrNull(TargetIndex.A);
			wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			wait.AddEndCondition((Func<JobCondition>)delegate
			{
				if (!((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_00f5: stateMachine*/)._003C_003Ef__this.GetComp((Pawn)(Thing)((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_00f5: stateMachine*/)._003C_003Ef__this.CurJob.GetTarget(TargetIndex.A)).ActiveAndFull)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			wait.defaultCompleteMode = ToilCompleteMode.Never;
			wait.WithProgressBar(TargetIndex.A, (Func<float>)(() => ((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_0119: stateMachine*/)._003C_003Ef__this.gatherProgress / ((_003CMakeNewToils_003Ec__Iterator4)/*Error near IL_0119: stateMachine*/)._003C_003Ef__this.WorkTotal), false, -0.5f);
			yield return wait;
		}
	}
}
