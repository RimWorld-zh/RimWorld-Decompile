using System;

namespace Verse
{
	// Token: 0x02000C88 RID: 3208
	public static class RegionAndRoomQuery
	{
		// Token: 0x06004655 RID: 18005 RVA: 0x00251AAC File Offset: 0x0024FEAC
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

		// Token: 0x06004656 RID: 18006 RVA: 0x00251AFC File Offset: 0x0024FEFC
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

		// Token: 0x06004657 RID: 18007 RVA: 0x00251B38 File Offset: 0x0024FF38
		public static Room RoomAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region region = RegionAndRoomQuery.RegionAt(c, map, allowedRegionTypes);
			return (region == null) ? null : region.Room;
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x00251B68 File Offset: 0x0024FF68
		public static RoomGroup RoomGroupAt(IntVec3 c, Map map)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, RegionType.Set_All);
			return (room == null) ? null : room.Group;
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x00251B98 File Offset: 0x0024FF98
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

		// Token: 0x0600465A RID: 18010 RVA: 0x00251BD4 File Offset: 0x0024FFD4
		public static RoomGroup GetRoomGroup(this Thing thing)
		{
			Room room = thing.GetRoom(RegionType.Set_All);
			return (room == null) ? null : room.Group;
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x00251C04 File Offset: 0x00250004
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

		// Token: 0x0600465C RID: 18012 RVA: 0x00251C48 File Offset: 0x00250048
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
