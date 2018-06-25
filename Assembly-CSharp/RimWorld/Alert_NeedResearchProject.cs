using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A4 RID: 1956
	public class Alert_NeedResearchProject : Alert
	{
		// Token: 0x06002B49 RID: 11081 RVA: 0x0016E1C5 File Offset: 0x0016C5C5
		public Alert_NeedResearchProject()
		{
			this.defaultLabel = "NeedResearchProject".Translate();
			this.defaultExplanation = "NeedResearchProjectDesc".Translate();
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x0016E1F0 File Offset: 0x0016C5F0
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
