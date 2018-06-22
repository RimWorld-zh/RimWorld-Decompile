using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C21 RID: 3105
	public static class GridsUtility
	{
		// Token: 0x06004406 RID: 17414 RVA: 0x0023DD24 File Offset: 0x0023C124
		public static float GetTemperature(this IntVec3 loc, Map map)
		{
			return GenTemperature.GetTemperatureForCell(loc, map);
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x0023DD40 File Offset: 0x0023C140
		public static Region GetRegion(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x0023DD60 File Offset: 0x0023C160
		public static Room GetRoom(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x0023DD80 File Offset: 0x0023C180
		public static RoomGroup GetRoomGroup(this IntVec3 loc, Map map)
		{
			return RegionAndRoomQuery.RoomGroupAt(loc, map);
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x0023DD9C File Offset: 0x0023C19C
		public static Room GetRoomOrAdjacent(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAtOrAdjacent(loc, map, allowedRegionTypes);
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x0023DDBC File Offset: 0x0023C1BC
		public static List<Thing> GetThingList(this IntVec3 c, Map map)
		{
			return map.thingGrid.ThingsListAt(c);
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x0023DDE0 File Offset: 0x0023C1E0
		public static float GetSnowDepth(this IntVec3 c, Map map)
		{
			return map.snowGrid.GetDepth(c);
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x0023DE04 File Offset: 0x0023C204
		public static bool Fogged(this Thing t)
		{
			return t.Map.fogGrid.IsFogged(t.Position);
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x0023DE30 File Offset: 0x0023C230
		public static bool Fogged(this IntVec3 c, Map map)
		{
			return map.fogGrid.IsFogged(c);
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x0023DE54 File Offset: 0x0023C254
		public static RoofDef GetRoof(this IntVec3 c, Map map)
		{
			return map.roofGrid.RoofAt(c);
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x0023DE78 File Offset: 0x0023C278
		public static bool Roofed(this IntVec3 c, Map map)
		{
			return map.roofGrid.Roofed(c);
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x0023DE9C File Offset: 0x0023C29C
		public static bool Filled(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x0023DED0 File Offset: 0x0023C2D0
		public static TerrainDef GetTerrain(this IntVec3 c, Map map)
		{
			return map.terrainGrid.TerrainAt(c);
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x0023DEF4 File Offset: 0x0023C2F4
		public static Zone GetZone(this IntVec3 c, Map map)
		{
			return map.zoneManager.ZoneAt(c);
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x0023DF18 File Offset: 0x0023C318
		public static Plant GetPlant(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Plant)
				{
					return (Plant)list[i];
				}
			}
			return null;
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x0023DF80 File Offset: 0x0023C380
		public static Thing GetRoofHolderOrImpassable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.holdsRoof || thingList[i].def.passability == Traversability.Impassable)
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x0023DFF4 File Offset: 0x0023C3F4
		public static Thing GetFirstThing(this IntVec3 c, Map map, ThingDef def)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == def)
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x0023E04C File Offset: 0x0023C44C
		public static ThingWithComps GetFirstThing<TComp>(this IntVec3 c, Map map) where TComp : ThingComp
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].TryGetComp<TComp>() != null)
				{
					return (ThingWithComps)thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x0023E0AC File Offset: 0x0023C4AC
		public static Thing GetFirstHaulable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.designateHaulable)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x0023E10C File Offset: 0x0023C50C
		public static Thing GetFirstItem(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x0023E16C File Offset: 0x0023C56C
		public static Building GetFirstBuilding(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Building building = list[i] as Building;
				if (building != null)
				{
					return building;
				}
			}
			return null;
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x0023E1C4 File Offset: 0x0023C5C4
		public static Pawn GetFirstPawn(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x0023E218 File Offset: 0x0023C618
		public static Mineable GetFirstMineable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Mineable mineable = thingList[i] as Mineable;
				if (mineable != null)
				{
					return mineable;
				}
			}
			return null;
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x0023E26C File Offset: 0x0023C66C
		public static Blight GetFirstBlight(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Blight blight = thingList[i] as Blight;
				if (blight != null)
				{
					return blight;
				}
			}
			return null;
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x0023E2C0 File Offset: 0x0023C6C0
		public static Skyfaller GetFirstSkyfaller(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Skyfaller skyfaller = thingList[i] as Skyfaller;
				if (skyfaller != null)
				{
					return skyfaller;
				}
			}
			return null;
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x0023E314 File Offset: 0x0023C714
		public static IPlantToGrowSettable GetPlantToGrowSettable(this IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetEdifice(map) as IPlantToGrowSettable;
			if (plantToGrowSettable == null)
			{
				plantToGrowSettable = (c.GetZone(map) as IPlantToGrowSettable);
			}
			return plantToGrowSettable;
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x0023E34C File Offset: 0x0023C74C
		public static Building GetTransmitter(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.EverTransmitsPower)
				{
					return (Building)list[i];
				}
			}
			return null;
		}

		// Token: 0x06004421 RID: 17441 RVA: 0x0023E3B0 File Offset: 0x0023C7B0
		public static Building_Door GetDoor(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Building_Door building_Door = list[i] as Building_Door;
				if (building_Door != null)
				{
					return building_Door;
				}
			}
			return null;
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x0023E408 File Offset: 0x0023C808
		public static Building GetEdifice(this IntVec3 c, Map map)
		{
			return map.edificeGrid[c];
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x0023E42C File Offset: 0x0023C82C
		public static Thing GetCover(this IntVec3 c, Map map)
		{
			return map.coverGrid[c];
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x0023E450 File Offset: 0x0023C850
		public static Gas GetGas(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Gas)
				{
					return (Gas)thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x0023E4B0 File Offset: 0x0023C8B0
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
				Log.Error("Checking prison cell status of " + c + " which is not in or adjacent to a room.", false);
				result = false;
			}
			return result;
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x0023E500 File Offset: 0x0023C900
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
					IntVec3[] array = GenAdj.CellsAdjacent8Way(edifice).ToArray<IntVec3>();
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].InBounds(map))
						{
							room = array[i].GetRoom(map, RegionType.Set_All);
							if (room != null && room.UsesOutdoorTemperature)
							{
								return true;
							}
						}
					}
					result = false;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
	}
}
