using System;

namespace Verse
{
	// Token: 0x02000B6A RID: 2922
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x06003FD8 RID: 16344 RVA: 0x0021A624 File Offset: 0x00218A24
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
