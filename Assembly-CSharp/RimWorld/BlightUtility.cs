using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BA RID: 1722
	public static class BlightUtility
	{
		// Token: 0x06002509 RID: 9481 RVA: 0x0013DEBC File Offset: 0x0013C2BC
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

		// Token: 0x0600250A RID: 9482 RVA: 0x0013DF18 File Offset: 0x0013C318
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
