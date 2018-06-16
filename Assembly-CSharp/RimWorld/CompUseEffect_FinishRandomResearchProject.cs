using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000764 RID: 1892
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		// Token: 0x060029BF RID: 10687 RVA: 0x00161FBC File Offset: 0x001603BC
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			IEnumerable<ResearchProjectDef> availableResearchProjects = this.GetAvailableResearchProjects();
			if (availableResearchProjects.Any<ResearchProjectDef>())
			{
				this.FinishInstantly(availableResearchProjects.RandomElement<ResearchProjectDef>());
			}
			else if (Find.ResearchManager.currentProj != null && !Find.ResearchManager.currentProj.IsFinished)
			{
				this.FinishInstantly(Find.ResearchManager.currentProj);
			}
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x00162028 File Offset: 0x00160428
		private IEnumerable<ResearchProjectDef> GetAvailableResearchProjects()
		{
			List<ResearchProjectDef> researchProjects = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < researchProjects.Count; i++)
			{
				if (researchProjects[i] != Find.ResearchManager.currentProj || Find.ResearchManager.currentProj.ProgressPercent < 0.2f)
				{
					if (!researchProjects[i].IsFinished && researchProjects[i].PrerequisitesCompleted)
					{
						yield return researchProjects[i];
					}
				}
			}
			yield break;
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x0016204B File Offset: 0x0016044B
		private void FinishInstantly(ResearchProjectDef proj)
		{
			Find.ResearchManager.InstantFinish(proj, false);
			Messages.Message("MessageResearchProjectFinishedByItem".Translate(new object[]
			{
				proj.LabelCap
			}), MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
