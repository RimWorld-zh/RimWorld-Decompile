using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public class SettlementUtility
	{
		public static bool IsPlayerAttackingAnySettlementOf(Faction faction)
		{
			bool result;
			if (faction == Faction.OfPlayer)
			{
				result = false;
			}
			else if (!faction.HostileTo(Faction.OfPlayer))
			{
				result = false;
			}
			else
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Settlement settlement = maps[i].info.parent as Settlement;
					if (settlement != null && settlement.Faction == faction)
						goto IL_0061;
				}
				result = false;
			}
			goto IL_0080;
			IL_0061:
			result = true;
			goto IL_0080;
			IL_0080:
			return result;
		}
	}
}
