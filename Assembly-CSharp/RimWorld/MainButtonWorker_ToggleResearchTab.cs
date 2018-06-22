using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000866 RID: 2150
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060030F3 RID: 12531 RVA: 0x001A9C14 File Offset: 0x001A8014
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
