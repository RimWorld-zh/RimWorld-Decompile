using System;

namespace Verse
{
	// Token: 0x02000B66 RID: 2918
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x06003FD9 RID: 16345 RVA: 0x0021AC8C File Offset: 0x0021908C
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
