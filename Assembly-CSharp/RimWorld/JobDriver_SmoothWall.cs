using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000048 RID: 72
	public class JobDriver_SmoothWall : JobDriver
	{
		// Token: 0x040001DC RID: 476
		private float workLeft = -1000f;

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000252 RID: 594 RVA: 0x000186C8 File Offset: 0x00016AC8
		protected int BaseWorkAmount
		{
			get
			{
				return 6500;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000253 RID: 595 RVA: 0x000186E4 File Offset: 0x00016AE4
		protected DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothWall;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00018700 File Offset: 0x00016B00
		protected StatDef SpeedStat
		{
			get
			{
				return StatDefOf.SmoothingSpeed;
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0001871C File Offset: 0x00016B1C
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null) && this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00018784 File Offset: 0x00016B84
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

		// Token: 0x06000257 RID: 599 RVA: 0x000187B0 File Offset: 0x00016BB0
		protected void DoEffect()
		{
			SmoothableWallUtility.Notify_SmoothedByPawn(SmoothableWallUtility.SmoothWall(base.TargetA.Thing, this.pawn), this.pawn);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x000187E2 File Offset: 0x00016BE2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}
	}
}
