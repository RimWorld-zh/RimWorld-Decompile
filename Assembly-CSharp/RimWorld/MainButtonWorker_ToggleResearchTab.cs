using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000868 RID: 2152
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060030F7 RID: 12535 RVA: 0x001A9D64 File Offset: 0x001A8164
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
