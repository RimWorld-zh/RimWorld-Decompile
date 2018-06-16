using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000064 RID: 100
	public class JobDriver_CleanFilth : JobDriver
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0001E36C File Offset: 0x0001C76C
		private Filth Filth
		{
			get
			{
				return (Filth)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0001E39C File Offset: 0x0001C79C
		public override bool TryMakePreToilReservations()
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0001E3D4 File Offset: 0x0001C7D4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, null);
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue).JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
			Toil clean = new Toil();
			clean.initAction = delegate()
			{
				this.cleaningWorkDone = 0f;
				this.totalCleaningWorkDone = 0f;
				this.totalCleaningWorkRequired = this.Filth.def.filth.cleaningWorkToReduceThickness * (float)this.Filth.thickness;
			};
			clean.tickAction = delegate()
			{
				Filth filth = this.Filth;
				this.cleaningWorkDone += 1f;
				this.totalCleaningWorkDone += 1f;
				if (this.cleaningWorkDone > filth.def.filth.cleaningWorkToReduceThickness)
				{
					filth.ThinFilth();
					this.cleaningWorkDone = 0f;
					if (filth.Destroyed)
					{
						clean.actor.records.Increment(RecordDefOf.MessesCleaned);
						this.ReadyForNextToil();
					}
				}
			};
			clean.defaultCompleteMode = ToilCompleteMode.Never;
			clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A);
			clean.WithProgressBar(TargetIndex.A, () => this.totalCleaningWorkDone / this.totalCleaningWorkRequired, true, -0.5f);
			clean.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
			clean.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
			clean.JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
			yield return clean;
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0001E400 File Offset: 0x0001C800
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.cleaningWorkDone, "cleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkDone, "totalCleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkRequired, "totalCleaningWorkRequired", 0f, false);
		}

		// Token: 0x04000201 RID: 513
		private float cleaningWorkDone = 0f;

		// Token: 0x04000202 RID: 514
		private float totalCleaningWorkDone = 0f;

		// Token: 0x04000203 RID: 515
		private float totalCleaningWorkRequired = 0f;

		// Token: 0x04000204 RID: 516
		private const TargetIndex FilthInd = TargetIndex.A;
	}
}
