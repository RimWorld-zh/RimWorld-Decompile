using System;

namespace Verse
{
	// Token: 0x02000B69 RID: 2921
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x06003FDC RID: 16348 RVA: 0x0021B048 File Offset: 0x00219448
		public int CompletedProjects()
		{
			int num = 0;
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				if (researchProjectDef.IsFinished && researchProjectDef.HasTag(this))
				{
					num++;
				}
			}
			return num;
		}
	}
}
