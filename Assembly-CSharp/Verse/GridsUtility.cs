using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C25 RID: 3109
	public static class GridsUtility
	{
		// Token: 0x060043FF RID: 17407 RVA: 0x0023C984 File Offset: 0x0023AD84
		public static float GetTemperature(this IntVec3 loc, Map map)
		{
			return GenTemperature.GetTemperatureForCell(loc, map);
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x0023C9A0 File Offset: 0x0023ADA0
		public static Region GetRegion(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x0023C9C0 File Offset: 0x0023ADC0
		public static Room GetRoom(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x0023C9E0 File Offset: 0x0023ADE0
		public static RoomGroup GetRoomGroup(this IntVec3 loc, Map map)
		{
			return RegionAndRoomQuery.RoomGroupAt(loc, map);
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x0023C9FC File Offset: 0x0023ADFC
		public static Room GetRoomOrAdjacent(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAtOrAdjacent(loc, map, allowedRegionTypes);
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x0023CA1C File Offset: 0x0023AE1C
		public static List<Thing> GetThingList(this IntVec3 c, Map map)
		{
			return map.thingGrid.ThingsListAt(c);
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x0023CA40 File Offset: 0x0023AE40
		public static float GetSnowDepth(this IntVec3 c, Map map)
		{
			return map.snowGrid.GetDepth(c);
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x0023CA64 File Offset: 0x0023AE64
		public static bool Fogged(this Thing t)
		{
			return t.Map.fogGrid.IsFogged(t.Position);
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x0023CA90 File Offset: 0x0023AE90
		public static bool Fogged(this IntVec3 c, Map map)
		{
			return map.fogGrid.IsFogged(c);
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x0023CAB4 File Offset: 0x0023AEB4
		public static RoofDef GetRoof(this IntVec3 c, Map map)
		{
			return map.roofGrid.RoofAt(c);
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x0023CAD8 File Offset: 0x0023AED8
		public static bool Roofed(this IntVec3 c, Map map)
		{
			return map.roofGrid.Roofed(c);
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x0023CAFC File Offset: 0x0023AEFC
		public static bool Filled(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.Fillage == FillCategory.Full;
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x0023CB30 File Offset: 0x0023AF30
		public static TerrainDef GetTerrain(this IntVec3 c, Map map)
		{
			return map.terrainGrid.TerrainAt(c);
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x0023CB54 File Offset: 0x0023AF54
		public static Zone GetZone(this IntVec3 c, Map map)
		{
			return map.zoneManager.ZoneAt(c);
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x0023CB78 File Offset: 0x0023AF78
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

		// Token: 0x0600440E RID: 17422 RVA: 0x0023CBE0 File Offset: 0x0023AFE0
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

		// Token: 0x0600440F RID: 17423 RVA: 0x0023CC54 File Offset: 0x0023B054
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

		// Token: 0x06004410 RID: 17424 RVA: 0x0023CCAC File Offset: 0x0023B0AC
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

		// Token: 0x06004411 RID: 17425 RVA: 0x0023CD0C File Offset: 0x0023B10C
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

		// Token: 0x06004412 RID: 17426 RVA: 0x0023CD6C File Offset: 0x0023B16C
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

		// Token: 0x06004413 RID: 17427 RVA: 0x0023CDCC File Offset: 0x0023B1CC
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

		// Token: 0x06004414 RID: 17428 RVA: 0x0023CE24 File Offset: 0x0023B224
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

		// Token: 0x06004415 RID: 17429 RVA: 0x0023CE78 File Offset: 0x0023B278
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

		// Token: 0x06004416 RID: 17430 RVA: 0x0023CECC File Offset: 0x0023B2CC
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

		// Token: 0x06004417 RID: 17431 RVA: 0x0023CF20 File Offset: 0x0023B320
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

		// Token: 0x06004418 RID: 17432 RVA: 0x0023CF74 File Offset: 0x0023B374
		public static IPlantToGrowSettable GetPlantToGrowSettable(this IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetEdifice(map) as IPlantToGrowSettable;
			if (plantToGrowSettable == null)
			{
				plantToGrowSettable = (c.GetZone(map) as IPlantToGrowSettable);
			}
			return plantToGrowSettable;
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x0023CFAC File Offset: 0x0023B3AC
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

		// Token: 0x0600441A RID: 17434 RVA: 0x0023D010 File Offset: 0x0023B410
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

		// Token: 0x0600441B RID: 17435 RVA: 0x0023D068 File Offset: 0x0023B468
		public static Building GetEdifice(this IntVec3 c, Map map)
		{
			return map.edificeGrid[c];
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x0023D08C File Offset: 0x0023B48C
		public static Thing GetCover(this IntVec3 c, Map map)
		{
			return map.coverGrid[c];
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x0023D0B0 File Offset: 0x0023B4B0
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

		// Token: 0x0600441E RID: 17438 RVA: 0x0023D110 File Offset: 0x0023B510
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

		// Token: 0x0600441F RID: 17439 RVA: 0x0023D160 File Offset: 0x0023B560
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
