using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000327 RID: 807
	public static class DeepDrillInfestationIncidentUtility
	{
		// Token: 0x06000DCA RID: 3530 RVA: 0x000760B0 File Offset: 0x000744B0
		public static void GetUsableDeepDrills(Map map, List<Thing> outDrills)
		{
			outDrills.Clear();
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.CreatesInfestations);
			Faction ofPlayer = Faction.OfPlayer;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == ofPlayer)
				{
					if (list[i].TryGetComp<CompCreatesInfestations>().CanCreateInfestationNow)
					{
						outDrills.Add(list[i]);
					}
				}
			}
		}
	}
}
