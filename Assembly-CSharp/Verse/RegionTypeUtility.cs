using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C98 RID: 3224
	public static class RegionTypeUtility
	{
		// Token: 0x060046B0 RID: 18096 RVA: 0x00253CDC File Offset: 0x002520DC
		public static bool IsOneCellRegion(this RegionType regionType)
		{
			return regionType == RegionType.Portal;
		}

		// Token: 0x060046B1 RID: 18097 RVA: 0x00253CF8 File Offset: 0x002520F8
		public static bool AllowsMultipleRegionsPerRoom(this RegionType regionType)
		{
			return regionType != RegionType.Portal;
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x00253D14 File Offset: 0x00252114
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

		// Token: 0x060046B3 RID: 18099 RVA: 0x00253DA4 File Offset: 0x002521A4
		public static RegionType GetRegionType(this IntVec3 c, Map map)
		{
			Region region = c.GetRegion(map, RegionType.Set_All);
			return (region == null) ? RegionType.None : region.type;
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x00253DD4 File Offset: 0x002521D4
		public static bool Passable(this RegionType regionType)
		{
			return (regionType & RegionType.Set_Passable) != RegionType.None;
		}
	}
}
