using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004F1 RID: 1265
	public static class BeautyUtility
	{
		// Token: 0x04000D50 RID: 3408
		public static List<IntVec3> beautyRelevantCells = new List<IntVec3>();

		// Token: 0x04000D51 RID: 3409
		private static List<Room> visibleRooms = new List<Room>();

		// Token: 0x04000D52 RID: 3410
		public static readonly int SampleNumCells_Beauty = GenRadial.NumCellsInRadius(8.9f);

		// Token: 0x04000D53 RID: 3411
		private static List<Thing> tempCountedThings = new List<Thing>();

		// Token: 0x060016C7 RID: 5831 RVA: 0x000C9BD4 File Offset: 0x000C7FD4
		public static float AverageBeautyPerceptible(IntVec3 root, Map map)
		{
			float result;
			if (!root.IsValid || !root.InBounds(map))
			{
				result = 0f;
			}
			else
			{
				BeautyUtility.tempCountedThings.Clear();
				float num = 0f;
				int num2 = 0;
				BeautyUtility.FillBeautyRelevantCells(root, map);
				for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
				{
					num += BeautyUtility.CellBeauty(BeautyUtility.beautyRelevantCells[i], map, BeautyUtility.tempCountedThings);
					num2++;
				}
				BeautyUtility.tempCountedThings.Clear();
				if (num2 == 0)
				{
					result = 0f;
				}
				else
				{
					result = num / (float)num2;
				}
			}
			return result;
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x000C9C80 File Offset: 0x000C8080
		public static void FillBeautyRelevantCells(IntVec3 root, Map map)
		{
			BeautyUtility.beautyRelevantCells.Clear();
			Room room = root.GetRoom(map, RegionType.Set_Passable);
			if (room != null)
			{
				BeautyUtility.visibleRooms.Clear();
				BeautyUtility.visibleRooms.Add(room);
				if (room.Regions.Count == 1 && room.Regions[0].type == RegionType.Portal)
				{
					foreach (Region region in room.Regions[0].Neighbors)
					{
						if (!BeautyUtility.visibleRooms.Contains(region.Room))
						{
							BeautyUtility.visibleRooms.Add(region.Room);
						}
					}
				}
				for (int i = 0; i < BeautyUtility.SampleNumCells_Beauty; i++)
				{
					IntVec3 intVec = root + GenRadial.RadialPattern[i];
					if (intVec.InBounds(map) && !intVec.Fogged(map))
					{
						Room room2 = intVec.GetRoom(map, RegionType.Set_Passable);
						if (!BeautyUtility.visibleRooms.Contains(room2))
						{
							bool flag = false;
							for (int j = 0; j < 8; j++)
							{
								IntVec3 loc = intVec + GenAdj.AdjacentCells[j];
								if (BeautyUtility.visibleRooms.Contains(loc.GetRoom(map, RegionType.Set_Passable)))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								goto IL_192;
							}
						}
						BeautyUtility.beautyRelevantCells.Add(intVec);
					}
					IL_192:;
				}
				BeautyUtility.visibleRooms.Clear();
			}
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x000C9E48 File Offset: 0x000C8248
		public static float CellBeauty(IntVec3 c, Map map, List<Thing> countedThings = null)
		{
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (BeautyUtility.BeautyRelevant(thing.def.category))
				{
					if (countedThings != null)
					{
						if (countedThings.Contains(thing))
						{
							goto IL_EE;
						}
						countedThings.Add(thing);
					}
					SlotGroup slotGroup = thing.GetSlotGroup();
					if (slotGroup == null || !slotGroup.parent.IgnoreStoredThingsBeauty)
					{
						float num3 = thing.GetStatValue(StatDefOf.Beauty, true);
						if (thing is Filth && !map.roofGrid.Roofed(c))
						{
							num3 *= 0.3f;
						}
						if (thing.def.Fillage == FillCategory.Full)
						{
							flag = true;
							num2 += num3;
						}
						else
						{
							num += num3;
						}
					}
				}
				IL_EE:;
			}
			float result;
			if (flag)
			{
				result = num2;
			}
			else
			{
				num += map.terrainGrid.TerrainAt(c).GetStatValueAbstract(StatDefOf.Beauty, null);
				result = num;
			}
			return result;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x000C9F8C File Offset: 0x000C838C
		public static bool BeautyRelevant(ThingCategory cat)
		{
			return cat == ThingCategory.Building || cat == ThingCategory.Item || cat == ThingCategory.Plant || cat == ThingCategory.Filth;
		}
	}
}
