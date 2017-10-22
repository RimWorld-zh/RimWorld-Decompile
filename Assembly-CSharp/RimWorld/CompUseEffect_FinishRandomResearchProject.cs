using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			IEnumerable<ResearchProjectDef> availableResearchProjects = this.GetAvailableResearchProjects();
			if (availableResearchProjects.Any())
			{
				this.FinishInstantly(availableResearchProjects.RandomElement());
			}
			else if (Find.ResearchManager.currentProj != null && !Find.ResearchManager.currentProj.IsFinished)
			{
				this.FinishInstantly(Find.ResearchManager.currentProj);
			}
		}

		private IEnumerable<ResearchProjectDef> GetAvailableResearchProjects()
		{
			List<ResearchProjectDef> researchProjects = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			int i = 0;
			while (true)
			{
				if (i < researchProjects.Count)
				{
					if ((researchProjects[i] != Find.ResearchManager.currentProj || !(Find.ResearchManager.currentProj.ProgressPercent >= 0.20000000298023224)) && !researchProjects[i].IsFinished && researchProjects[i].PrerequisitesCompleted)
						break;
					i++;
					continue;
				}
				yield break;
			}
			yield return researchProjects[i];
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private void FinishInstantly(ResearchProjectDef proj)
		{
			Find.ResearchManager.InstantFinish(proj, false);
			Messages.Message("MessageResearchProjectFinishedByItem".Translate(proj.label), MessageTypeDefOf.PositiveEvent);
		}
	}
}
