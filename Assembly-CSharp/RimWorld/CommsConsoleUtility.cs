using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class CommsConsoleUtility
	{
		public static bool PlayerHasPoweredCommsConsole(Map map)
		{
			List<Thing> list = map.listerThings.ThingsMatching(ThingRequest.ForDef(ThingDefOf.CommsConsole));
			int num = 0;
			bool result;
			while (true)
			{
				if (num < list.Count)
				{
					if (list[num].Faction == Faction.OfPlayer)
					{
						CompPowerTrader compPowerTrader = list[num].TryGetComp<CompPowerTrader>();
						if (compPowerTrader != null && !compPowerTrader.PowerOn)
						{
							goto IL_0064;
						}
						result = true;
						break;
					}
					goto IL_0064;
				}
				result = false;
				break;
				IL_0064:
				num++;
			}
			return result;
		}

		public static bool PlayerHasPoweredCommsConsole()
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < maps.Count)
				{
					if (CommsConsoleUtility.PlayerHasPoweredCommsConsole(maps[num]))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
