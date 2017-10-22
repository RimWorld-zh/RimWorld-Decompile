using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_NeedResearchProject : Alert
	{
		public Alert_NeedResearchProject()
		{
			base.defaultLabel = "NeedResearchProject".Translate();
			base.defaultExplanation = "NeedResearchProjectDesc".Translate();
		}

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
				int num = 0;
				while (num < maps.Count)
				{
					if (!maps[num].IsPlayerHome || !maps[num].listerBuildings.ColonistsHaveResearchBench())
					{
						num++;
						continue;
					}
					flag = true;
					break;
				}
				result = (flag ? (Find.ResearchManager.AnyProjectIsAvailable ? true : false) : false);
			}
			return result;
		}
	}
}
