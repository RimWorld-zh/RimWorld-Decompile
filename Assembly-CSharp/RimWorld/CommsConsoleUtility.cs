using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A4 RID: 1700
	public static class CommsConsoleUtility
	{
		// Token: 0x06002429 RID: 9257 RVA: 0x001365C8 File Offset: 0x001349C8
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

		// Token: 0x0600242A RID: 9258 RVA: 0x00136654 File Offset: 0x00134A54
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
