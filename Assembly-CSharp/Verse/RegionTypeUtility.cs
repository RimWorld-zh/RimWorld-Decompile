using System.Collections.Generic;

namespace Verse
{
	public static class RegionTypeUtility
	{
		public static bool IsOneCellRegion(this RegionType regionType)
		{
			return regionType == RegionType.Portal;
		}

		public static bool AllowsMultipleRegionsPerRoom(this RegionType regionType)
		{
			return regionType != RegionType.Portal;
		}

		public static RegionType GetExpectedRegionType(this IntVec3 c, Map map)
		{
			RegionType result;
			if (!c.InBounds(map))
			{
				result = RegionType.None;
			}
			else if (c.GetDoor(map) != null)
			{
				result = RegionType.Portal;
			}
			else if (c.Walkable(map))
			{
				result = RegionType.Normal;
			}
			else
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def.Fillage == FillCategory.Full)
						goto IL_0061;
				}
				result = RegionType.ImpassableFreeAirExchange;
			}
			goto IL_0080;
			IL_0061:
			result = RegionType.None;
			goto IL_0080;
			IL_0080:
			return result;
		}

		public static RegionType GetRegionType(this IntVec3 c, Map map)
		{
			Region region = c.GetRegion(map, RegionType.Set_All);
			return (region != null) ? region.type : RegionType.None;
		}

		public static bool Passable(this RegionType regionType)
		{
			return ((int)regionType & 6) != 0;
		}
	}
}
