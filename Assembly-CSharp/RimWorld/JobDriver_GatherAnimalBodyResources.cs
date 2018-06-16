using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000031 RID: 49
	public abstract class JobDriver_GatherAnimalBodyResources : JobDriver
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001C0 RID: 448
		protected abstract float WorkTotal { get; }

		// Token: 0x060001C1 RID: 449
		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		// Token: 0x060001C2 RID: 450 RVA: 0x00012FE9 File Offset: 0x000113E9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.gatherProgress, "gatherProgress", 0f, false);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00013008 File Offset: 0x00011408
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00013040 File Offset: 0x00011440
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil wait = new Toil();
			wait.initAction = delegate()
			{
				Pawn actor = wait.actor;
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				actor.pather.StopDead();
				PawnUtility.ForceWait(pawn, 15000, null, true);
			};
			wait.tickAction = delegate()
			{
				Pawn actor = wait.actor;
				actor.skills.Learn(SkillDefOf.Animals, 0.142999992f, false);
				this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true);
				if (this.gatherProgress >= this.WorkTotal)
				{
					this.GetComp((Pawn)((Thing)this.job.GetTarget(TargetIndex.A))).Gathered(this.pawn);
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
				}
			};
			wait.AddFinishAction(delegate
			{
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				if (pawn != null && pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				}
			});
			wait.FailOnDespawnedOrNull(TargetIndex.A);
			wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			wait.AddEndCondition(delegate
			{
				JobCondition result;
				if (!this.GetComp((Pawn)((Thing)this.job.GetTarget(TargetIndex.A))).ActiveAndFull)
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			wait.defaultCompleteMode = ToilCompleteMode.Never;
			wait.WithProgressBar(TargetIndex.A, () => this.gatherProgress / this.WorkTotal, false, -0.5f);
			wait.activeSkill = (() => SkillDefOf.Animals);
			yield return wait;
			yield break;
		}

		// Token: 0x040001B6 RID: 438
		private float gatherProgress = 0f;

		// Token: 0x040001B7 RID: 439
		protected const TargetIndex AnimalInd = TargetIndex.A;
	}
}
