using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A2 RID: 1698
	public static class CommsConsoleUtility
	{
		// Token: 0x06002426 RID: 9254 RVA: 0x00136B40 File Offset: 0x00134F40
		public static bool PlayerHasPoweredCommsConsole(Map map)
		{
			List<Thing> list = map.listerThings.ThingsMatching(ThingRequest.ForDef(ThingDefOf.CommsConsole));
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == Faction.OfPlayer)
				{
					CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x00136BCC File Offset: 0x00134FCC
		public static bool PlayerHasPoweredCommsConsole()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (CommsConsoleUtility.PlayerHasPoweredCommsConsole(maps[i]))
				{
					return true;
				}
			}
			return false;
		}
	}
}
