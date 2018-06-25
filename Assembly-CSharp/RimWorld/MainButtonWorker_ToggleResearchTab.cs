using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000868 RID: 2152
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060030F6 RID: 12534 RVA: 0x001A9FCC File Offset: 0x001A83CC
		public override float ButtonBarPercent
		{
			get
			{
				ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
				float result;
				if (currentProj == null)
				{
					result = 0f;
				}
				else
				{
					result = currentProj.ProgressPercent;
				}
				return result;
			}
		}
	}
}
