using System;

namespace Verse
{
	// Token: 0x02000C89 RID: 3209
	public static class RegionAndRoomQuery
	{
		// Token: 0x06004655 RID: 18005 RVA: 0x00251D8C File Offset: 0x0025018C
		public static Region RegionAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region result;
			if (!c.InBounds(map))
			{
				result = null;
			}
			else
			{
				Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
				if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
				{
					result = validRegionAt;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x00251DDC File Offset: 0x002501DC
		public static Region GetRegion(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region result;
			if (!thing.Spawned)
			{
				result = null;
			}
			else
			{
				result = RegionAndRoomQuery.RegionAt(thing.Position, thing.Map, allowedRegionTypes);
			}
			return result;
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x00251E18 File Offset: 0x00250218
		public static Room RoomAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region region = RegionAndRoomQuery.RegionAt(c, map, allowedRegionTypes);
			return (region == null) ? null : region.Room;
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x00251E48 File Offset: 0x00250248
		public static RoomGroup RoomGroupAt(IntVec3 c, Map map)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, RegionType.Set_All);
			return (room == null) ? null : room.Group;
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x00251E78 File Offset: 0x00250278
		public static Room GetRoom(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Room result;
			if (!thing.Spawned)
			{
				result = null;
			}
			else
			{
				result = RegionAndRoomQuery.RoomAt(thing.Position, thing.Map, allowedRegionTypes);
			}
			return result;
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x00251EB4 File Offset: 0x002502B4
		public static RoomGroup GetRoomGroup(this Thing thing)
		{
			Room room = thing.GetRoom(RegionType.Set_All);
			return (room == null) ? null : room.Group;
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x00251EE4 File Offset: 0x002502E4
		public static Room RoomAtFast(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			Room result;
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				result = validRegionAt.Room;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x00251F28 File Offset: 0x00250328
		public static Room RoomAtOrAdjacent(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, allowedRegionTypes);
			Room result;
			if (room != null)
			{
				result = room;
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCells[i];
					room = RegionAndRoomQuery.RoomAt(c2, map, allowedRegionTypes);
					if (room != null)
					{
						return room;
					}
				}
				result = room;
			}
			return result;
		}
	}
}
