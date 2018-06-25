using System;

namespace Verse
{
	public static class RegionAndRoomQuery
	{
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

		public static Room RoomAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region region = RegionAndRoomQuery.RegionAt(c, map, allowedRegionTypes);
			return (region == null) ? null : region.Room;
		}

		public static RoomGroup RoomGroupAt(IntVec3 c, Map map)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, RegionType.Set_All);
			return (room == null) ? null : room.Group;
		}

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

		public static RoomGroup GetRoomGroup(this Thing thing)
		{
			Room room = thing.GetRoom(RegionType.Set_All);
			return (room == null) ? null : room.Group;
		}

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
