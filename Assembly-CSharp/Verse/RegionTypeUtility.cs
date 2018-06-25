using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C97 RID: 3223
	public static class RegionTypeUtility
	{
		// Token: 0x060046BC RID: 18108 RVA: 0x002551A8 File Offset: 0x002535A8
		public static bool IsOneCellRegion(this RegionType regionType)
		{
			return regionType == RegionType.Portal;
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x002551C4 File Offset: 0x002535C4
		public static bool AllowsMultipleRegionsPerRoom(this RegionType regionType)
		{
			return regionType != RegionType.Portal;
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x002551E0 File Offset: 0x002535E0
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

		// Token: 0x060046BF RID: 18111 RVA: 0x00255270 File Offset: 0x00253670
		public static RegionType GetRegionType(this IntVec3 c, Map map)
		{
			Region region = c.GetRegion(map, RegionType.Set_All);
			return (region == null) ? RegionType.None : region.type;
		}

		// Token: 0x060046C0 RID: 18112 RVA: 0x002552A0 File Offset: 0x002536A0
		public static bool Passable(this RegionType regionType)
		{
			return (regionType & RegionType.Set_Passable) != RegionType.None;
		}
	}
}
