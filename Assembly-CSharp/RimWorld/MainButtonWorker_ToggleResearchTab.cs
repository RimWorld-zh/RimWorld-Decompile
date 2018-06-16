using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086A RID: 2154
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060030F8 RID: 12536 RVA: 0x001A9964 File Offset: 0x001A7D64
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
