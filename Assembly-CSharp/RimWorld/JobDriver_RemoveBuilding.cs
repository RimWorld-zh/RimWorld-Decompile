using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000045 RID: 69
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00017198 File Offset: 0x00015598
		protected Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600023F RID: 575 RVA: 0x000171C0 File Offset: 0x000155C0
		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000240 RID: 576
		protected abstract DesignationDef Designation { get; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000241 RID: 577
		protected abstract int TotalNeededWork { get; }

		// Token: 0x06000242 RID: 578 RVA: 0x000171E5 File Offset: 0x000155E5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0001721C File Offset: 0x0001561C
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00017250 File Offset: 0x00015650
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnThingMissingDesignation(TargetIndex.A, this.Designation);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil().FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.initAction = delegate()
			{
				this.totalNeededWork = (float)this.TotalNeededWork;
				this.workLeft = this.totalNeededWork;
			};
			doWork.tickAction = delegate()
			{
				this.workLeft -= this.pawn.GetStatValue(StatDefOf.ConstructionSpeed, true);
				this.TickAction();
				if (this.workLeft <= 0f)
				{
					doWork.actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / this.totalNeededWork, false, -0.5f);
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield return new Toil
			{
				initAction = delegate()
				{
					this.FinishedRemoving();
					base.Map.designationManager.RemoveAllDesignationsOn(this.Target, false);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0001727A File Offset: 0x0001567A
		protected virtual void FinishedRemoving()
		{
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0001727D File Offset: 0x0001567D
		protected virtual void TickAction()
		{
		}

		// Token: 0x040001D6 RID: 470
		private float workLeft = 0f;

		// Token: 0x040001D7 RID: 471
		private float totalNeededWork = 0f;
	}
}
