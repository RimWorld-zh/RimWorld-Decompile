using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000760 RID: 1888
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		// Token: 0x060029BA RID: 10682 RVA: 0x00162228 File Offset: 0x00160628
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

		// Token: 0x060029BB RID: 10683 RVA: 0x00162294 File Offset: 0x00160694
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

		// Token: 0x060029BC RID: 10684 RVA: 0x001622B7 File Offset: 0x001606B7
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
