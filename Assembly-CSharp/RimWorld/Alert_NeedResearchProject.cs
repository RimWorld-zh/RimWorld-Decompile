using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A2 RID: 1954
	public class Alert_NeedResearchProject : Alert
	{
		// Token: 0x06002B46 RID: 11078 RVA: 0x0016DE11 File Offset: 0x0016C211
		public Alert_NeedResearchProject()
		{
			this.defaultLabel = "NeedResearchProject".Translate();
			this.defaultExplanation = "NeedResearchProjectDesc".Translate();
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x0016DE3C File Offset: 0x0016C23C
		public override AlertReport GetReport()
		{
			AlertReport result;
			if (Find.AnyPlayerHomeMap == null)
			{
				result = false;
			}
			else if (Find.ResearchManager.currentProj != null)
			{
				result = false;
			}
			else
			{
				bool flag = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						if (maps[i].listerBuildings.ColonistsHaveResearchBench())
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					result = false;
				}
				else if (!Find.ResearchManager.AnyProjectIsAvailable)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
	}
}
