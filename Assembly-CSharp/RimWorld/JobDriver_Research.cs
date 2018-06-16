using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200007C RID: 124
	public class JobDriver_Research : JobDriver
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00024AAC File Offset: 0x00022EAC
		private ResearchProjectDef Project
		{
			get
			{
				return Find.ResearchManager.currentProj;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00024ACC File Offset: 0x00022ECC
		private Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)base.TargetThingA;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00024AEC File Offset: 0x00022EEC
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00024B20 File Offset: 0x00022F20
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil research = new Toil();
			research.tickAction = delegate()
			{
				Pawn actor = research.actor;
				float num = 1.1f * actor.GetStatValue(StatDefOf.ResearchSpeed, true);
				num *= this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
				Find.ResearchManager.ResearchPerformed(num, actor);
				actor.skills.Learn(SkillDefOf.Intellectual, 0.11f, false);
				actor.GainComfortFromCellIfPossible();
			};
			research.FailOn(() => this.Project == null);
			research.FailOn(() => !this.Project.CanBeResearchedAt(this.ResearchBench, false));
			research.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			research.WithEffect(EffecterDefOf.Research, TargetIndex.A);
			research.WithProgressBar(TargetIndex.A, delegate
			{
				ResearchProjectDef project = this.Project;
				float result;
				if (project == null)
				{
					result = 0f;
				}
				else
				{
					result = project.ProgressPercent;
				}
				return result;
			}, false, -0.5f);
			research.defaultCompleteMode = ToilCompleteMode.Delay;
			research.defaultDuration = 4000;
			research.activeSkill = (() => SkillDefOf.Intellectual);
			yield return research;
			yield return Toils_General.Wait(2);
			yield break;
		}

		// Token: 0x04000231 RID: 561
		private const int JobEndInterval = 4000;

		// Token: 0x04000232 RID: 562
		private const float BaseResearchSpeed = 1.1f;
	}
}
