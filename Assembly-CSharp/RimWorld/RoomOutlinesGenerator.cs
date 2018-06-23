using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020003E6 RID: 998
	public static class RoomOutlinesGenerator
	{
		// Token: 0x04000A5A RID: 2650
		private const int MinFreeRoomCellsToDivide = 32;

		// Token: 0x04000A5B RID: 2651
		private const int MinAllowedRoomWidthAndHeight = 2;

		// Token: 0x06001116 RID: 4374 RVA: 0x00092560 File Offset: 0x00090960
		public static List<RoomOutline> GenerateRoomOutlines(CellRect initialRect, Map map, int divisionsCount, int finalRoomsCount, int maxRoomCells, int minTotalRoomsNonWallCellsCount)
		{
			int num = 0;
			List<RoomOutline> list;
			for (;;)
			{
				list = RoomOutlinesGenerator.GenerateRoomOutlines(initialRect, map, divisionsCount, finalRoomsCount, maxRoomCells);
				int num2 = 0;
				for (int i = 0; i < list.Count; i++)
				{
					num2 += list[i].CellsCountIgnoringWalls;
				}
				if (num2 >= minTotalRoomsNonWallCellsCount)
				{
					break;
				}
				num++;
				if (num > 15)
				{
					goto Block_3;
				}
			}
			return list;
			Block_3:
			return list;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x000925D4 File Offset: 0x000909D4
		public static List<RoomOutline> GenerateRoomOutlines(CellRect initialRect, Map map, int divisionsCount, int finalRoomsCount, int maxRoomCells)
		{
			List<RoomOutline> list = new List<RoomOutline>();
			list.Add(new RoomOutline(initialRect));
			for (int i = 0; i < divisionsCount; i++)
			{
				RoomOutline roomOutline;
				if (!(from x in list
				where x.CellsCountIgnoringWalls >= 32
				select x).TryRandomElementByWeight((RoomOutline x) => (float)Mathf.Max(x.rect.Width, x.rect.Height), out roomOutline))
				{
					break;
				}
				bool flag = roomOutline.rect.Height > roomOutline.rect.Width;
				if (!flag || roomOutline.rect.Height > 6)
				{
					if (flag || roomOutline.rect.Width > 6)
					{
						RoomOutlinesGenerator.Split(roomOutline, list, flag);
					}
				}
			}
			while (list.Any((RoomOutline x) => x.CellsCountIgnoringWalls > maxRoomCells))
			{
				RoomOutline roomOutline2 = (from x in list
				where x.CellsCountIgnoringWalls > maxRoomCells
				select x).RandomElement<RoomOutline>();
				bool horizontalWall = roomOutline2.rect.Height > roomOutline2.rect.Width;
				RoomOutlinesGenerator.Split(roomOutline2, list, horizontalWall);
			}
			while (list.Count > finalRoomsCount)
			{
				list.Remove(list.RandomElement<RoomOutline>());
			}
			return list;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0009274C File Offset: 0x00090B4C
		private static void Split(RoomOutline room, List<RoomOutline> allRooms, bool horizontalWall)
		{
			allRooms.Remove(room);
			if (horizontalWall)
			{
				int z = room.rect.CenterCell.z;
				allRooms.Add(new RoomOutline(new CellRect(room.rect.minX, room.rect.minZ, room.rect.Width, z - room.rect.minZ + 1)));
				allRooms.Add(new RoomOutline(new CellRect(room.rect.minX, z, room.rect.Width, room.rect.maxZ - z + 1)));
			}
			else
			{
				int x = room.rect.CenterCell.x;
				allRooms.Add(new RoomOutline(new CellRect(room.rect.minX, room.rect.minZ, x - room.rect.minX + 1, room.rect.Height)));
				allRooms.Add(new RoomOutline(new CellRect(x, room.rect.minZ, room.rect.maxX - x + 1, room.rect.Height)));
			}
		}
	}
}
