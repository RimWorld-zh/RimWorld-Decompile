using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000047 RID: 71
	public class JobDriver_Repair : JobDriver
	{
		// Token: 0x040001D9 RID: 473
		protected float ticksToNextRepair = 0f;

		// Token: 0x040001DA RID: 474
		private const float WarmupTicks = 80f;

		// Token: 0x040001DB RID: 475
		private const float TicksBetweenRepairs = 20f;

		// Token: 0x0600024F RID: 591 RVA: 0x00018290 File Offset: 0x00016690
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000182C4 File Offset: 0x000166C4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil repair = new Toil();
			repair.initAction = delegate()
			{
				this.ticksToNextRepair = 80f;
			};
			repair.tickAction = delegate()
			{
				Pawn actor = repair.actor;
				actor.skills.Learn(SkillDefOf.Construction, 0.275f, false);
				float statValue = actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				this.ticksToNextRepair -= statValue;
				if (this.ticksToNextRepair <= 0f)
				{
					this.ticksToNextRepair += 20f;
					this.TargetThingA.HitPoints++;
					this.TargetThingA.HitPoints = Mathf.Min(this.TargetThingA.HitPoints, this.TargetThingA.MaxHitPoints);
					this.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)this.TargetThingA);
					if (this.TargetThingA.HitPoints == this.TargetThingA.MaxHitPoints)
					{
						actor.records.Increment(RecordDefOf.ThingsRepaired);
						actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
					}
				}
			};
			repair.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			repair.WithEffect(base.TargetThingA.def.repairEffect, TargetIndex.A);
			repair.defaultCompleteMode = ToilCompleteMode.Never;
			repair.activeSkill = (() => SkillDefOf.Construction);
			yield return repair;
			yield break;
		}
	}
}
