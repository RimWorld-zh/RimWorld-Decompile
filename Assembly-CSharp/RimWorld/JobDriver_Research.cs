using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Research : JobDriver
	{
		private const int JobEndInterval = 4000;

		private ResearchProjectDef Project
		{
			get
			{
				return Find.ResearchManager.currentProj;
			}
		}

		private Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)base.TargetThingA;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil research = new Toil
			{
				tickAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator3B)/*Error near IL_007f: stateMachine*/)._003Cresearch_003E__0.actor;
					float statValue = actor.GetStatValue(StatDefOf.ResearchSpeed, true);
					statValue *= ((_003CMakeNewToils_003Ec__Iterator3B)/*Error near IL_007f: stateMachine*/)._003C_003Ef__this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
					Find.ResearchManager.ResearchPerformed(statValue, actor);
					actor.skills.Learn(SkillDefOf.Intellectual, 0.11f, false);
					actor.GainComfortFromCellIfPossible();
				}
			};
			research.FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator3B)/*Error near IL_0096: stateMachine*/)._003C_003Ef__this.Project == null));
			research.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator3B)/*Error near IL_00ae: stateMachine*/)._003C_003Ef__this.Project.CanBeResearchedAt(((_003CMakeNewToils_003Ec__Iterator3B)/*Error near IL_00ae: stateMachine*/)._003C_003Ef__this.ResearchBench, false)));
			research.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			research.WithEffect(EffecterDefOf.Research, TargetIndex.A);
			research.WithProgressBar(TargetIndex.A, (Func<float>)delegate
			{
				ResearchProjectDef project = ((_003CMakeNewToils_003Ec__Iterator3B)/*Error near IL_00e7: stateMachine*/)._003C_003Ef__this.Project;
				if (project == null)
				{
					return 0f;
				}
				return project.ProgressPercent;
			}, false, -0.5f);
			research.defaultCompleteMode = ToilCompleteMode.Delay;
			research.defaultDuration = 4000;
			yield return research;
			yield return Toils_General.Wait(2);
		}
	}
}
