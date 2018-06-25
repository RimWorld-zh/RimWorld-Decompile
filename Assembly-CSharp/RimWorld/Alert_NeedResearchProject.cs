using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_NeedResearchProject : Alert
	{
		public Alert_NeedResearchProject()
		{
			this.defaultLabel = "NeedResearchProject".Translate();
			this.defaultExplanation = "NeedResearchProjectDesc".Translate();
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
