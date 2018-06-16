using System;

namespace Verse
{
	// Token: 0x02000B6A RID: 2922
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x06003FD6 RID: 16342 RVA: 0x0021A550 File Offset: 0x00218950
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
