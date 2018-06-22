using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A0 RID: 1696
	public static class CommsConsoleUtility
	{
		// Token: 0x06002423 RID: 9251 RVA: 0x00136788 File Offset: 0x00134B88
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

		// Token: 0x06002424 RID: 9252 RVA: 0x00136814 File Offset: 0x00134C14
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
