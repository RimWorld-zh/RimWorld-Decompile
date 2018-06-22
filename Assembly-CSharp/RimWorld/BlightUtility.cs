using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B6 RID: 1718
	public static class BlightUtility
	{
		// Token: 0x06002501 RID: 9473 RVA: 0x0013E004 File Offset: 0x0013C404
		public static Plant GetFirstBlightableNowPlant(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Plant plant = thingList[i] as Plant;
				if (plant != null && plant.BlightableNow)
				{
					return plant;
				}
			}
			return null;
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x0013E060 File Offset: 0x0013C460
		public static Plant GetFirstBlightableEverPlant(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Plant plant = thingList[i] as Plant;
				if (plant != null && plant.def.plant.Blightable)
				{
					return plant;
				}
			}
			return null;
		}
	}
}
