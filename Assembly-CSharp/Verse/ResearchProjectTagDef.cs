using System;

namespace Verse
{
	// Token: 0x02000B68 RID: 2920
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x06003FDC RID: 16348 RVA: 0x0021AD68 File Offset: 0x00219168
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
