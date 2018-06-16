using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C99 RID: 3225
	public static class RegionTypeUtility
	{
		// Token: 0x060046B2 RID: 18098 RVA: 0x00253D04 File Offset: 0x00252104
		public static bool IsOneCellRegion(this RegionType regionType)
		{
			return regionType == RegionType.Portal;
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x00253D20 File Offset: 0x00252120
		public static bool AllowsMultipleRegionsPerRoom(this RegionType regionType)
		{
			return regionType != RegionType.Portal;
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x00253D3C File Offset: 0x0025213C
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
					{
						return RegionType.None;
					}
				}
				result = RegionType.ImpassableFreeAirExchange;
			}
			return result;
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x00253DCC File Offset: 0x002521CC
		public static RegionType GetRegionType(this IntVec3 c, Map map)
		{
			Region region = c.GetRegion(map, RegionType.Set_All);
			return (region == null) ? RegionType.None : region.type;
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x00253DFC File Offset: 0x002521FC
		public static bool Passable(this RegionType regionType)
		{
			return (regionType & RegionType.Set_Passable) != RegionType.None;
		}
	}
}
