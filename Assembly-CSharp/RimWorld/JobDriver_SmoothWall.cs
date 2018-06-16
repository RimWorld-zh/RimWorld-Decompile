using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000048 RID: 72
	public class JobDriver_SmoothWall : JobDriver
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000252 RID: 594 RVA: 0x000186C0 File Offset: 0x00016AC0
		protected int BaseWorkAmount
		{
			get
			{
				return 9000;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000253 RID: 595 RVA: 0x000186DC File Offset: 0x00016ADC
		protected DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothWall;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000254 RID: 596 RVA: 0x000186F8 File Offset: 0x00016AF8
		protected StatDef SpeedStat
		{
			get
			{
				return StatDefOf.SmoothingSpeed;
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00018714 File Offset: 0x00016B14
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null) && this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0001877C File Offset: 0x00016B7C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !this.job.ignoreDesignations && this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef) == null);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
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
				if (this.workLeft <= 0f)
				{
					this.DoEffect();
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

		// Token: 0x06000257 RID: 599 RVA: 0x000187A8 File Offset: 0x00016BA8
		protected void DoEffect()
		{
			SmoothableWallUtility.Notify_SmoothedByPawn(SmoothableWallUtility.SmoothWall(base.TargetA.Thing, this.pawn), this.pawn);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x000187DA File Offset: 0x00016BDA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x040001DC RID: 476
		private float workLeft = -1000f;
	}
}
