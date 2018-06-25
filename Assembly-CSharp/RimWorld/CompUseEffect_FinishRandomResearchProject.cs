using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000762 RID: 1890
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		// Token: 0x060029BE RID: 10686 RVA: 0x00162378 File Offset: 0x00160778
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

		// Token: 0x060029BF RID: 10687 RVA: 0x001623E4 File Offset: 0x001607E4
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

		// Token: 0x060029C0 RID: 10688 RVA: 0x00162407 File Offset: 0x00160807
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
