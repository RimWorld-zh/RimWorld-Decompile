using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000329 RID: 809
	public static class DeepDrillInfestationIncidentUtility
	{
		// Token: 0x06000DCD RID: 3533 RVA: 0x00076208 File Offset: 0x00074608
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
