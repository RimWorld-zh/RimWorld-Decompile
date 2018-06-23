using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200003C RID: 60
	public abstract class JobDriver_AffectFloor : JobDriver
	{
		// Token: 0x040001CA RID: 458
		private float workLeft = -1000f;

		// Token: 0x040001CB RID: 459
		protected bool clearSnow = false;

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600020A RID: 522
		protected abstract int BaseWorkAmount { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600020B RID: 523
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00015DA4 File Offset: 0x000141A4
		protected virtual StatDef SpeedStat
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00015DBC File Offset: 0x000141BC
		public override bool TryMakePreToilReservations()
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo targetA = this.job.targetA;
			Job job = this.job;
			ReservationLayerDef floor = ReservationLayerDefOf.Floor;
			return pawn.Reserve(targetA, job, 1, -1, floor);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00015E00 File Offset: 0x00014200
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !this.job.ignoreDesignations && this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef) == null);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate()
			{
				this.workLeft = (float)this.BaseWorkAmount;
			};
			doWork.tickAction = delegate()
			{
				float num = (this.SpeedStat == null) ? 1f : doWork.actor.GetStatValue(this.SpeedStat, true);
				this.workLeft -= num;
				if (doWork.actor.skills != null)
				{
					doWork.actor.skills.Learn(SkillDefOf.Construction, 0.11f, false);
				}
				if (this.clearSnow)
				{
					this.Map.snowGrid.SetDepth(this.TargetLocA, 0f);
				}
				if (this.workLeft <= 0f)
				{
					this.DoEffect(this.TargetLocA);
					Designation designation = this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef);
					if (designation != null)
					{
						designation.Delete();
					}
					this.ReadyForNextToil();
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / (float)this.BaseWorkAmount, false, -0.5f);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x0600020F RID: 527
		protected abstract void DoEffect(IntVec3 c);

		// Token: 0x06000210 RID: 528 RVA: 0x00015E2A File Offset: 0x0001422A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}
	}
}
