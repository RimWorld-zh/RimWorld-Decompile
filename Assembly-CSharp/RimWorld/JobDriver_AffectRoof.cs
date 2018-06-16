using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200003F RID: 63
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00016438 File Offset: 0x00014838
		protected IntVec3 Cell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600021D RID: 541
		protected abstract PathEndMode PathEndMode { get; }

		// Token: 0x0600021E RID: 542
		protected abstract void DoEffect();

		// Token: 0x0600021F RID: 543
		protected abstract bool DoWorkFailOn();

		// Token: 0x06000220 RID: 544 RVA: 0x00016461 File Offset: 0x00014861
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00016480 File Offset: 0x00014880
		public override bool TryMakePreToilReservations()
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.Cell;
			Job job = this.job;
			ReservationLayerDef ceiling = ReservationLayerDefOf.Ceiling;
			return pawn.Reserve(target, job, 1, -1, ceiling);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000164C4 File Offset: 0x000148C4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_Goto.Goto(TargetIndex.B, this.PathEndMode);
			Toil doWork = new Toil();
			doWork.initAction = delegate()
			{
				this.workLeft = 65f;
			};
			doWork.tickAction = delegate()
			{
				float statValue = doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				this.workLeft -= statValue;
				if (this.workLeft <= 0f)
				{
					this.DoEffect();
					this.ReadyForNextToil();
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.B, this.PathEndMode);
			doWork.PlaySoundAtStart(SoundDefOf.Roof_Start);
			doWork.PlaySoundAtEnd(SoundDefOf.Roof_Finish);
			doWork.WithEffect(EffecterDefOf.RoofWork, TargetIndex.A);
			doWork.FailOn(new Func<bool>(this.DoWorkFailOn));
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / 65f, false, -0.5f);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			yield return doWork;
			yield break;
		}

		// Token: 0x040001CC RID: 460
		private float workLeft;

		// Token: 0x040001CD RID: 461
		private const TargetIndex CellInd = TargetIndex.A;

		// Token: 0x040001CE RID: 462
		private const TargetIndex GotoTargetInd = TargetIndex.B;

		// Token: 0x040001CF RID: 463
		private const float BaseWorkAmount = 65f;
	}
}
