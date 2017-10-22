using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class GridsUtility
	{
		public static float GetTemperature(this IntVec3 loc, Map map)
		{
			return GenTemperature.GetTemperatureForCell(loc, map);
		}

		public static Region GetRegion(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(loc, map, allowedRegionTypes);
		}

		public static Room GetRoom(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAt(loc, map, allowedRegionTypes);
		}

		public static RoomGroup GetRoomGroup(this IntVec3 loc, Map map)
		{
			return RegionAndRoomQuery.RoomGroupAt(loc, map);
		}

		public static Room GetRoomOrAdjacent(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAtOrAdjacent(loc, map, allowedRegionTypes);
		}

		public static List<Thing> GetThingList(this IntVec3 c, Map map)
		{
			return map.thingGrid.ThingsListAt(c);
		}

		public static float GetSnowDepth(this IntVec3 c, Map map)
		{
			return map.snowGrid.GetDepth(c);
		}

		public static bool Fogged(this IntVec3 c, Map map)
		{
			return map.fogGrid.IsFogged(c);
		}

		public static RoofDef GetRoof(this IntVec3 c, Map map)
		{
			return map.roofGrid.RoofAt(c);
		}

		public static bool Roofed(this IntVec3 c, Map map)
		{
			return map.roofGrid.Roofed(c);
		}

		public static bool Filled(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.Fillage == FillCategory.Full;
		}

		public static TerrainDef GetTerrain(this IntVec3 c, Map map)
		{
			return map.terrainGrid.TerrainAt(c);
		}

		public static Zone GetZone(this IntVec3 c, Map map)
		{
			return map.zoneManager.ZoneAt(c);
		}

		public static Plant GetPlant(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			Plant result;
			while (true)
			{
				if (num < list.Count)
				{
					if (list[num].def.category == ThingCategory.Plant)
					{
						result = (Plant)list[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Thing GetRoofHolderOrImpassable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < thingList.Count)
				{
					if (!thingList[num].def.holdsRoof && thingList[num].def.passability != Traversability.Impassable)
					{
						num++;
						continue;
					}
					result = thingList[num];
				}
				else
				{
					result = null;
				}
				break;
			}
			return result;
		}

		public static Thing GetFirstThing(this IntVec3 c, Map map, ThingDef def)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < thingList.Count)
				{
					if (thingList[num].def == def)
					{
						result = thingList[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Thing GetFirstHaulable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < list.Count)
				{
					if (list[num].def.designateHaulable)
					{
						result = list[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Thing GetFirstItem(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < list.Count)
				{
					if (list[num].def.category == ThingCategory.Item)
					{
						result = list[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Building GetFirstBuilding(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			Building result;
			while (true)
			{
				if (num < list.Count)
				{
					Building building = list[num] as Building;
					if (building != null)
					{
						result = building;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Pawn GetFirstPawn(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Pawn result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Pawn pawn = thingList[num] as Pawn;
					if (pawn != null)
					{
						result = pawn;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Mineable GetFirstMineable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Mineable result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Mineable mineable = thingList[num] as Mineable;
					if (mineable != null)
					{
						result = mineable;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Blight GetFirstBlight(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Blight result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Blight blight = thingList[num] as Blight;
					if (blight != null)
					{
						result = blight;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Skyfaller GetFirstSkyfaller(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Skyfaller result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Skyfaller skyfaller = thingList[num] as Skyfaller;
					if (skyfaller != null)
					{
						result = skyfaller;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static IPlantToGrowSettable GetPlantToGrowSettable(this IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetEdifice(map) as IPlantToGrowSettable;
			if (plantToGrowSettable == null)
			{
				plantToGrowSettable = (c.GetZone(map) as IPlantToGrowSettable);
			}
			return plantToGrowSettable;
		}

		public static Building GetTransmitter(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			Building result;
			while (true)
			{
				if (num < list.Count)
				{
					if (list[num].def.EverTransmitsPower)
					{
						result = (Building)list[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Building_Door GetDoor(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			Building_Door result;
			while (true)
			{
				if (num < list.Count)
				{
					Building_Door building_Door = list[num] as Building_Door;
					if (building_Door != null)
					{
						result = building_Door;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Building GetEdifice(this IntVec3 c, Map map)
		{
			return map.edificeGrid[c];
		}

		public static Thing GetCover(this IntVec3 c, Map map)
		{
			return map.coverGrid[c];
		}

		public static Gas GetGas(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Gas result;
			while (true)
			{
				if (num < thingList.Count)
				{
					if (thingList[num].def.category == ThingCategory.Gas)
					{
						result = (Gas)thingList[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static bool IsInPrisonCell(this IntVec3 c, Map map)
		{
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_Passable);
			bool result;
			if (roomOrAdjacent != null)
			{
				result = roomOrAdjacent.isPrisonCell;
			}
			else
			{
				Log.Error("Checking prison cell status of " + c + " which is not in or adjacent to a room.");
				result = false;
			}
			return result;
		}

		public static bool UsesOutdoorTemperature(this IntVec3 c, Map map)
		{
			Room room = c.GetRoom(map, RegionType.Set_All);
			bool result;
			if (room != null)
			{
				result = room.UsesOutdoorTemperature;
			}
			else
			{
				Building edifice = c.GetEdifice(map);
				if (edifice != null)
				{
					IntVec3[] array = GenAdj.CellsAdjacent8Way(edifice).ToArray();
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].InBounds(map))
						{
							room = array[i].GetRoom(map, RegionType.Set_All);
							if (room != null && room.UsesOutdoorTemperature)
								goto IL_0081;
						}
					}
					result = false;
				}
				else
				{
					result = false;
				}
			}
			goto IL_00a8;
			IL_0081:
			result = true;
			goto IL_00a8;
			IL_00a8:
			return result;
		}
	}
}
