using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A4 RID: 1700
	public static class CommsConsoleUtility
	{
		// Token: 0x0600242B RID: 9259 RVA: 0x00136640 File Offset: 0x00134A40
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

		// Token: 0x0600242C RID: 9260 RVA: 0x001366CC File Offset: 0x00134ACC
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
