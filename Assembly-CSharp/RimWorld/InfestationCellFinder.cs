using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class InfestationCellFinder
	{
		private struct LocationCandidate
		{
			public IntVec3 cell;

			public float score;

			public LocationCandidate(IntVec3 cell, float score)
			{
				this.cell = cell;
				this.score = score;
			}
		}

		private const float MinRequiredScore = 7.5f;

		private const float MinMountainousnessScore = 0.17f;

		private const int MountainousnessScoreRadialPatternIdx = 700;

		private const int MountainousnessScoreRadialPatternSkip = 10;

		private const float MountainousnessScorePerRock = 1f;

		private const float MountainousnessScorePerThickRoof = 0.5f;

		private const float MinCellTempToSpawnHive = -17f;

		private const float MaxDistanceToColonyBuilding = 30f;

		private static List<LocationCandidate> locationCandidates = new List<LocationCandidate>();

		private static Dictionary<Region, float> regionsDistanceToUnroofed = new Dictionary<Region, float>();

		private static ByteGrid closedAreaSize;

		private static ByteGrid distToColonyBuilding;

		private static HashSet<Region> tempUnroofedRegions = new HashSet<Region>();

		private static List<IntVec3> tmpColonyBuildingsLocs = new List<IntVec3>();

		private static List<KeyValuePair<IntVec3, float>> tmpDistanceResult = new List<KeyValuePair<IntVec3, float>>();

		public static bool TryFindCell(out IntVec3 cell, Map map)
		{
			InfestationCellFinder.CalculateLocationCandidates(map);
			LocationCandidate locationCandidate = default(LocationCandidate);
			if (!((IEnumerable<LocationCandidate>)InfestationCellFinder.locationCandidates).TryRandomElementByWeight<LocationCandidate>((Func<LocationCandidate, float>)((LocationCandidate x) => x.score), out locationCandidate))
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = locationCandidate.cell;
			return true;
		}

		private static float GetScoreAt(IntVec3 cell, Map map)
		{
			if ((float)(int)InfestationCellFinder.distToColonyBuilding[cell] > 30.0)
			{
				return 0f;
			}
			if (!cell.Standable(map))
			{
				return 0f;
			}
			if (cell.Fogged(map))
			{
				return 0f;
			}
			if (InfestationCellFinder.CellHasBlockingThings(cell, map))
			{
				return 0f;
			}
			if (cell.Roofed(map) && cell.GetRoof(map).isThickRoof)
			{
				Region region = cell.GetRegion(map, RegionType.Set_Passable);
				if (region == null)
				{
					return 0f;
				}
				if (InfestationCellFinder.closedAreaSize[cell] < 16)
				{
					return 0f;
				}
				float temperature = cell.GetTemperature(map);
				if (temperature < -17.0)
				{
					return 0f;
				}
				float mountainousnessScoreAt = InfestationCellFinder.GetMountainousnessScoreAt(cell, map);
				if (mountainousnessScoreAt < 0.17000000178813934)
				{
					return 0f;
				}
				int num = InfestationCellFinder.StraightLineDistToUnroofed(cell, map);
				float num2 = (float)(InfestationCellFinder.regionsDistanceToUnroofed.TryGetValue(region, out num2) ? Mathf.Min(num2, (float)((float)num * 4.0)) : ((float)num * 1.1499999761581421));
				num2 = Mathf.Pow(num2, 1.55f);
				float num3 = Mathf.InverseLerp(0f, 12f, (float)num);
				float num4 = Mathf.Lerp(1f, 0.18f, map.glowGrid.GameGlowAt(cell));
				float num5 = (float)(1.0 - Mathf.Clamp((float)(InfestationCellFinder.DistToBlocker(cell, map) / 11.0), 0f, 0.6f));
				float num6 = Mathf.InverseLerp(-17f, -7f, temperature);
				float f = num2 * num3 * num5 * mountainousnessScoreAt * num4 * num6;
				f = Mathf.Pow(f, 1.2f);
				if (f < 7.5)
				{
					return 0f;
				}
				return f;
			}
			return 0f;
		}

		public static void DebugDraw()
		{
			if (DebugViewSettings.drawInfestationChance)
			{
				Map visibleMap = Find.VisibleMap;
				CellRect cellRect = Find.CameraDriver.CurrentViewRect;
				cellRect.ClipInsideMap(visibleMap);
				cellRect = cellRect.ExpandedBy(1);
				InfestationCellFinder.CalculateTraversalDistancesToUnroofed(visibleMap);
				InfestationCellFinder.CalculateClosedAreaSizeGrid(visibleMap);
				InfestationCellFinder.CalculateDistanceToColonyBuildingGrid(visibleMap);
				float num = 0.001f;
				int num2 = 0;
				while (true)
				{
					int num3 = num2;
					IntVec3 size = visibleMap.Size;
					if (num3 < size.z)
					{
						int num4 = 0;
						while (true)
						{
							int num5 = num4;
							IntVec3 size2 = visibleMap.Size;
							if (num5 < size2.x)
							{
								IntVec3 cell = new IntVec3(num4, 0, num2);
								float scoreAt = InfestationCellFinder.GetScoreAt(cell, visibleMap);
								if (scoreAt > num)
								{
									num = scoreAt;
								}
								num4++;
								continue;
							}
							break;
						}
						num2++;
						continue;
					}
					break;
				}
				int num6 = 0;
				while (true)
				{
					int num7 = num6;
					IntVec3 size3 = visibleMap.Size;
					if (num7 < size3.z)
					{
						int num8 = 0;
						while (true)
						{
							int num9 = num8;
							IntVec3 size4 = visibleMap.Size;
							if (num9 < size4.x)
							{
								IntVec3 intVec = new IntVec3(num8, 0, num6);
								if (cellRect.Contains(intVec))
								{
									float scoreAt2 = InfestationCellFinder.GetScoreAt(intVec, visibleMap);
									if (!(scoreAt2 <= 0.0))
									{
										float a = GenMath.LerpDouble(7.5f, num, 0f, 1f, scoreAt2);
										CellRenderer.RenderCell(intVec, SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 1f, a), false));
									}
								}
								num8++;
								continue;
							}
							break;
						}
						num6++;
						continue;
					}
					break;
				}
			}
		}

		private static void CalculateLocationCandidates(Map map)
		{
			InfestationCellFinder.locationCandidates.Clear();
			InfestationCellFinder.CalculateTraversalDistancesToUnroofed(map);
			InfestationCellFinder.CalculateClosedAreaSizeGrid(map);
			InfestationCellFinder.CalculateDistanceToColonyBuildingGrid(map);
			int num = 0;
			while (true)
			{
				int num2 = num;
				IntVec3 size = map.Size;
				if (num2 < size.z)
				{
					int num3 = 0;
					while (true)
					{
						int num4 = num3;
						IntVec3 size2 = map.Size;
						if (num4 < size2.x)
						{
							IntVec3 cell = new IntVec3(num3, 0, num);
							float scoreAt = InfestationCellFinder.GetScoreAt(cell, map);
							if (!(scoreAt <= 0.0))
							{
								InfestationCellFinder.locationCandidates.Add(new LocationCandidate(cell, scoreAt));
							}
							num3++;
							continue;
						}
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}

		private static bool CellHasBlockingThings(IntVec3 cell, Map map)
		{
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is Pawn)
				{
					return true;
				}
				if ((thingList[i].def.category == ThingCategory.Building || thingList[i].def.category == ThingCategory.Item) && GenSpawn.SpawningWipes(ThingDefOf.Hive, thingList[i].def))
				{
					return true;
				}
			}
			return false;
		}

		private static int StraightLineDistToUnroofed(IntVec3 cell, Map map)
		{
			int num = 2147483647;
			for (int i = 0; i < 4; i++)
			{
				int num2 = 0;
				IntVec3 facingCell = new Rot4(i).FacingCell;
				int num3 = 0;
				while (true)
				{
					IntVec3 intVec = cell + facingCell * num3;
					if (!intVec.InBounds(map))
					{
						num2 = 2147483647;
					}
					else
					{
						num2 = num3;
						if (!InfestationCellFinder.NoRoofAroundAndWalkable(intVec, map))
						{
							num3++;
							continue;
						}
					}
					break;
				}
				if (num2 < num)
				{
					num = num2;
				}
			}
			if (num == 2147483647)
			{
				IntVec3 size = map.Size;
				return size.x;
			}
			return num;
		}

		private static float DistToBlocker(IntVec3 cell, Map map)
		{
			int num = -2147483648;
			int num2 = -2147483648;
			for (int i = 0; i < 4; i++)
			{
				int num3 = 0;
				IntVec3 facingCell = new Rot4(i).FacingCell;
				int num4 = 0;
				while (true)
				{
					IntVec3 c = cell + facingCell * num4;
					num3 = num4;
					if (c.InBounds(map) && c.Walkable(map))
					{
						num4++;
						continue;
					}
					break;
				}
				if (num3 > num)
				{
					num2 = num;
					num = num3;
				}
				else if (num3 > num2)
				{
					num2 = num3;
				}
			}
			return (float)Mathf.Min(num, num2);
		}

		private static bool NoRoofAroundAndWalkable(IntVec3 cell, Map map)
		{
			if (!cell.Walkable(map))
			{
				return false;
			}
			if (cell.Roofed(map))
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = new Rot4(i).FacingCell + cell;
				if (c.InBounds(map) && c.Roofed(map))
				{
					return false;
				}
			}
			return true;
		}

		private static float GetMountainousnessScoreAt(IntVec3 cell, Map map)
		{
			float num = 0f;
			int num2 = 0;
			for (int num3 = 0; num3 < 700; num3 += 10)
			{
				IntVec3 c = cell + GenRadial.RadialPattern[num3];
				if (c.InBounds(map))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isNaturalRock)
					{
						num = (float)(num + 1.0);
					}
					else if (c.Roofed(map) && c.GetRoof(map).isThickRoof)
					{
						num = (float)(num + 0.5);
					}
					num2++;
				}
			}
			return num / (float)num2;
		}

		private static void CalculateTraversalDistancesToUnroofed(Map map)
		{
			InfestationCellFinder.tempUnroofedRegions.Clear();
			int num = 0;
			while (true)
			{
				int num2 = num;
				IntVec3 size = map.Size;
				if (num2 < size.z)
				{
					int num3 = 0;
					while (true)
					{
						int num4 = num3;
						IntVec3 size2 = map.Size;
						if (num4 < size2.x)
						{
							IntVec3 intVec = new IntVec3(num3, 0, num);
							Region region = intVec.GetRegion(map, RegionType.Set_Passable);
							if (region != null && InfestationCellFinder.NoRoofAroundAndWalkable(intVec, map))
							{
								InfestationCellFinder.tempUnroofedRegions.Add(region);
							}
							num3++;
							continue;
						}
						break;
					}
					num++;
					continue;
				}
				break;
			}
			Dijkstra<Region>.Run((IEnumerable<Region>)InfestationCellFinder.tempUnroofedRegions, (Func<Region, IEnumerable<Region>>)((Region x) => x.Neighbors), (Func<Region, Region, float>)((Region a, Region b) => Mathf.Sqrt((float)a.extentsClose.CenterCell.DistanceToSquared(b.extentsClose.CenterCell))), ref InfestationCellFinder.regionsDistanceToUnroofed);
			InfestationCellFinder.tempUnroofedRegions.Clear();
		}

		private static void CalculateClosedAreaSizeGrid(Map map)
		{
			if (InfestationCellFinder.closedAreaSize == null)
			{
				InfestationCellFinder.closedAreaSize = new ByteGrid(map);
			}
			else
			{
				InfestationCellFinder.closedAreaSize.ClearAndResizeTo(map);
			}
			int num = 0;
			while (true)
			{
				int num2 = num;
				IntVec3 size = map.Size;
				if (num2 < size.z)
				{
					int num3 = 0;
					while (true)
					{
						int num4 = num3;
						IntVec3 size2 = map.Size;
						if (num4 < size2.x)
						{
							IntVec3 intVec = new IntVec3(num3, 0, num);
							if (InfestationCellFinder.closedAreaSize[num3, num] == 0 && !intVec.Impassable(map))
							{
								int area = 0;
								map.floodFiller.FloodFill(intVec, (Predicate<IntVec3>)((IntVec3 c) => !c.Impassable(map)), (Action<IntVec3>)delegate(IntVec3 c)
								{
									area++;
								}, false);
								area = Mathf.Min(area, 255);
								map.floodFiller.FloodFill(intVec, (Predicate<IntVec3>)((IntVec3 c) => !c.Impassable(map)), (Action<IntVec3>)delegate(IntVec3 c)
								{
									InfestationCellFinder.closedAreaSize[c] = (byte)area;
								}, false);
							}
							num3++;
							continue;
						}
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}

		private static void CalculateDistanceToColonyBuildingGrid(Map map)
		{
			if (InfestationCellFinder.distToColonyBuilding == null)
			{
				InfestationCellFinder.distToColonyBuilding = new ByteGrid(map);
			}
			else if (!InfestationCellFinder.distToColonyBuilding.MapSizeMatches(map))
			{
				InfestationCellFinder.distToColonyBuilding.ClearAndResizeTo(map);
			}
			InfestationCellFinder.distToColonyBuilding.Clear((byte)255);
			InfestationCellFinder.tmpColonyBuildingsLocs.Clear();
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				InfestationCellFinder.tmpColonyBuildingsLocs.Add(allBuildingsColonist[i].Position);
			}
			Dijkstra<IntVec3>.Run((IEnumerable<IntVec3>)InfestationCellFinder.tmpColonyBuildingsLocs, (Func<IntVec3, IEnumerable<IntVec3>>)((IntVec3 x) => DijkstraUtility.AdjacentCellsNeighborsGetter(x, map)), (Func<IntVec3, IntVec3, float>)delegate(IntVec3 a, IntVec3 b)
			{
				if (a.x != b.x && a.z != b.z)
				{
					return 1.41421354f;
				}
				return 1f;
			}, ref InfestationCellFinder.tmpDistanceResult);
			for (int j = 0; j < InfestationCellFinder.tmpDistanceResult.Count; j++)
			{
				InfestationCellFinder.distToColonyBuilding[InfestationCellFinder.tmpDistanceResult[j].Key] = (byte)Mathf.Min(InfestationCellFinder.tmpDistanceResult[j].Value, 254.999f);
			}
		}
	}
}
