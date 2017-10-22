using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class BlightUtility
	{
		public static Plant GetFirstBlightableNowPlant(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Plant result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Plant plant = thingList[num] as Plant;
					if (plant != null && plant.BlightableNow)
					{
						result = plant;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Plant GetFirstBlightableEverPlant(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Plant result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Plant plant = thingList[num] as Plant;
					if (plant != null && plant.def.plant.Blightable)
					{
						result = plant;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
