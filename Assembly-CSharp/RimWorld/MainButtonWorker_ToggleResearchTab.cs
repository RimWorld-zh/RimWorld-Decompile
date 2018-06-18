using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086A RID: 2154
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060030FA RID: 12538 RVA: 0x001A9A2C File Offset: 0x001A7E2C
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
