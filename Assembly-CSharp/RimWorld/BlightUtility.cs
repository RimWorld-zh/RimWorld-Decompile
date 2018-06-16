using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BA RID: 1722
	public static class BlightUtility
	{
		// Token: 0x06002507 RID: 9479 RVA: 0x0013DE44 File Offset: 0x0013C244
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

		// Token: 0x06002508 RID: 9480 RVA: 0x0013DEA0 File Offset: 0x0013C2A0
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
