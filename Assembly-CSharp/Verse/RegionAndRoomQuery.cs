using System;

namespace Verse
{
	// Token: 0x02000C89 RID: 3209
	public static class RegionAndRoomQuery
	{
		// Token: 0x06004649 RID: 17993 RVA: 0x00250600 File Offset: 0x0024EA00
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

		// Token: 0x0600464A RID: 17994 RVA: 0x00250650 File Offset: 0x0024EA50
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

		// Token: 0x0600464B RID: 17995 RVA: 0x0025068C File Offset: 0x0024EA8C
		public static Room RoomAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region region = RegionAndRoomQuery.RegionAt(c, map, allowedRegionTypes);
			return (region == null) ? null : region.Room;
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x002506BC File Offset: 0x0024EABC
		public static RoomGroup RoomGroupAt(IntVec3 c, Map map)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, RegionType.Set_All);
			return (room == null) ? null : room.Group;
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x002506EC File Offset: 0x0024EAEC
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

		// Token: 0x0600464E RID: 17998 RVA: 0x00250728 File Offset: 0x0024EB28
		public static RoomGroup GetRoomGroup(this Thing thing)
		{
			Room room = thing.GetRoom(RegionType.Set_All);
			return (room == null) ? null : room.Group;
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x00250758 File Offset: 0x0024EB58
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

		// Token: 0x06004650 RID: 18000 RVA: 0x0025079C File Offset: 0x0024EB9C
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
