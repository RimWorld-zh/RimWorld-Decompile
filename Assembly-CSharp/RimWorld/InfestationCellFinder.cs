using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class InfestationCellFinder
	{
		private static List<InfestationCellFinder.LocationCandidate> locationCandidates = new List<InfestationCellFinder.LocationCandidate>();

		private static Dictionary<Region, float> regionsDistanceToUnroofed = new Dictionary<Region, float>();

		private static ByteGrid closedAreaSize;

		private static ByteGrid distToColonyBuilding;

		private const float MinRequiredScore = 7.5f;

		private const float MinMountainousnessScore = 0.17f;

		private const int MountainousnessScoreRadialPatternIdx = 700;

		private const int MountainousnessScoreRadialPatternSkip = 10;

		private const float MountainousnessScorePerRock = 1f;

		private const float MountainousnessScorePerThickRoof = 0.5f;

		private const float MinCellTempToSpawnHive = -17f;

		private const float MaxDistanceToColonyBuilding = 30f;

		private static List<Pair<IntVec3, float>> tmpCachedInfestationChanceCellColors;

		private static HashSet<Region> tempUnroofedRegions = new HashSet<Region>();

		private static List<IntVec3> tmpColonyBuildingsLocs = new List<IntVec3>();

		private static List<KeyValuePair<IntVec3, float>> tmpDistanceResult = new List<KeyValuePair<IntVec3, float>>();

		[CompilerGenerated]
		private static Func<InfestationCellFinder.LocationCandidate, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Region, IEnumerable<Region>> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Region, Region, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<IntVec3, IntVec3, float> <>f__am$cache3;

		public static bool TryFindCell(out IntVec3 cell, Map map)
		{
			InfestationCellFinder.CalculateLocationCandidates(map);
			InfestationCellFinder.LocationCandidate locationCandidate;
			bool result;
			if (!InfestationCellFinder.locationCandidates.TryRandomElementByWeight((InfestationCellFinder.LocationCandidate x) => x.score, out locationCandidate))
			{
				cell = IntVec3.Invalid;
				result = false;
			}
			else
			{
				cell = CellFinder.FindNoWipeSpawnLocNear(locationCandidate.cell, map, ThingDefOf.Hive, Rot4.North, 2, (IntVec3 x) => InfestationCellFinder.GetScoreAt(x, map) > 0f && x.GetFirstThing(map, ThingDefOf.Hive) == null && x.GetFirstThing(map, ThingDefOf.TunnelHiveSpawner) == null);
				result = true;
			}
			return result;
		}

		private static float GetScoreAt(IntVec3 cell, Map map)
		{
			float result;
			if ((float)InfestationCellFinder.distToColonyBuilding[cell] > 30f)
			{
				result = 0f;
			}
			else if (!cell.Walkable(map))
			{
				result = 0f;
			}
			else if (cell.Fogged(map))
			{
				result = 0f;
			}
			else if (InfestationCellFinder.CellHasBlockingThings(cell, map))
			{
				result = 0f;
			}
			else if (!cell.Roofed(map) || !cell.GetRoof(map).isThickRoof)
			{
				result = 0f;
			}
			else
			{
				Region region = cell.GetRegion(map, RegionType.Set_Passable);
				if (region == null)
				{
					result = 0f;
				}
				else if (InfestationCellFinder.closedAreaSize[cell] < 2)
				{
					result = 0f;
				}
				else
				{
					float temperature = cell.GetTemperature(map);
					if (temperature < -17f)
					{
						result = 0f;
					}
					else
					{
						float mountainousnessScoreAt = InfestationCellFinder.GetMountainousnessScoreAt(cell, map);
						if (mountainousnessScoreAt < 0.17f)
						{
							result = 0f;
						}
						else
						{
							int num = InfestationCellFinder.StraightLineDistToUnroofed(cell, map);
							float num2;
							if (!InfestationCellFinder.regionsDistanceToUnroofed.TryGetValue(region, out num2))
							{
								num2 = (float)num * 1.15f;
							}
							else
							{
								num2 = Mathf.Min(num2, (float)num * 4f);
							}
							num2 = Mathf.Pow(num2, 1.55f);
							float num3 = Mathf.InverseLerp(0f, 12f, (float)num);
							float num4 = Mathf.Lerp(1f, 0.18f, map.glowGrid.GameGlowAt(cell, false));
							float num5 = 1f - Mathf.Clamp(InfestationCellFinder.DistToBlocker(cell, map) / 11f, 0f, 0.6f);
							float num6 = Mathf.InverseLerp(-17f, -7f, temperature);
							float num7 = num2 * num3 * num5 * mountainousnessScoreAt * num4 * num6;
							num7 = Mathf.Pow(num7, 1.2f);
							if (num7 < 7.5f)
							{
								result = 0f;
							}
							else
							{
								result = num7;
							}
						}
					}
				}
			}
			return result;
		}

		public static void DebugDraw()
		{
			if (DebugViewSettings.drawInfestationChance)
			{
				if (InfestationCellFinder.tmpCachedInfestationChanceCellColors == null)
				{
					InfestationCellFinder.tmpCachedInfestationChanceCellColors = new List<Pair<IntVec3, float>>();
				}
				if (Time.frameCount % 8 == 0)
				{
					InfestationCellFinder.tmpCachedInfestationChanceCellColors.Clear();
					Map currentMap = Find.CurrentMap;
					CellRect cellRect = Find.CameraDriver.CurrentViewRect;
					cellRect.ClipInsideMap(currentMap);
					cellRect = cellRect.ExpandedBy(1);
					InfestationCellFinder.CalculateTraversalDistancesToUnroofed(currentMap);
					InfestationCellFinder.CalculateClosedAreaSizeGrid(currentMap);
					InfestationCellFinder.CalculateDistanceToColonyBuildingGrid(currentMap);
					float num = 0.001f;
					for (int i = 0; i < currentMap.Size.z; i++)
					{
						for (int j = 0; j < currentMap.Size.x; j++)
						{
							IntVec3 cell = new IntVec3(j, 0, i);
							float scoreAt = InfestationCellFinder.GetScoreAt(cell, currentMap);
							if (scoreAt > num)
							{
								num = scoreAt;
							}
						}
					}
					for (int k = 0; k < currentMap.Size.z; k++)
					{
						for (int l = 0; l < currentMap.Size.x; l++)
						{
							IntVec3 intVec = new IntVec3(l, 0, k);
							if (cellRect.Contains(intVec))
							{
								float scoreAt2 = InfestationCellFinder.GetScoreAt(intVec, currentMap);
								if (scoreAt2 > 7.5f)
								{
									float second = GenMath.LerpDouble(7.5f, num, 0f, 1f, scoreAt2);
									InfestationCellFinder.tmpCachedInfestationChanceCellColors.Add(new Pair<IntVec3, float>(intVec, second));
								}
							}
						}
					}
				}
				for (int m = 0; m < InfestationCellFinder.tmpCachedInfestationChanceCellColors.Count; m++)
				{
					IntVec3 first = InfestationCellFinder.tmpCachedInfestationChanceCellColors[m].First;
					float second2 = InfestationCellFinder.tmpCachedInfestationChanceCellColors[m].Second;
					CellRenderer.RenderCell(first, SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 1f, second2), false));
				}
			}
			else
			{
				InfestationCellFinder.tmpCachedInfestationChanceCellColors = null;
			}
		}

		private static void CalculateLocationCandidates(Map map)
		{
			InfestationCellFinder.locationCandidates.Clear();
			InfestationCellFinder.CalculateTraversalDistancesToUnroofed(map);
			InfestationCellFinder.CalculateClosedAreaSizeGrid(map);
			InfestationCellFinder.CalculateDistanceToColonyBuildingGrid(map);
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 cell = new IntVec3(j, 0, i);
					float scoreAt = InfestationCellFinder.GetScoreAt(cell, map);
					if (scoreAt > 0f)
					{
						InfestationCellFinder.locationCandidates.Add(new InfestationCellFinder.LocationCandidate(cell, scoreAt));
					}
				}
			}
		}

		private static bool CellHasBlockingThings(IntVec3 cell, Map map)
		{
			List<Thing> thingList = cell.GetThingList(map);
			int i = 0;
			while (i < thingList.Count)
			{
				bool result;
				if (thingList[i] is Pawn || thingList[i] is Hive || thingList[i] is TunnelHiveSpawner)
				{
					result = true;
				}
				else
				{
					bool flag = thingList[i].def.category == ThingCategory.Building && thingList[i].def.passability == Traversability.Impassable;
					if (!flag || !GenSpawn.SpawningWipes(ThingDefOf.Hive, thingList[i].def))
					{
						i++;
						continue;
					}
					result = true;
				}
				return result;
			}
			return false;
		}

		private static int StraightLineDistToUnroofed(IntVec3 cell, Map map)
		{
			int num = int.MaxValue;
			int i = 0;
			while (i < 4)
			{
				Rot4 rot = new Rot4(i);
				IntVec3 facingCell = rot.FacingCell;
				int num2 = 0;
				int num3;
				for (;;)
				{
					IntVec3 intVec = cell + facingCell * num2;
					if (!intVec.InBounds(map))
					{
						goto Block_1;
					}
					num3 = num2;
					if (InfestationCellFinder.NoRoofAroundAndWalkable(intVec, map))
					{
						break;
					}
					num2++;
				}
				IL_74:
				if (num3 < num)
				{
					num = num3;
				}
				i++;
				continue;
				Block_1:
				num3 = int.MaxValue;
				goto IL_74;
			}
			int result;
			if (num == 2147483647)
			{
				result = map.Size.x;
			}
			else
			{
				result = num;
			}
			return result;
		}

		private static float DistToBlocker(IntVec3 cell, Map map)
		{
			int num = int.MinValue;
			int num2 = int.MinValue;
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4(i);
				IntVec3 facingCell = rot.FacingCell;
				int num3 = 0;
				int num4;
				for (;;)
				{
					IntVec3 c = cell + facingCell * num3;
					num4 = num3;
					if (!c.InBounds(map) || !c.Walkable(map))
					{
						break;
					}
					num3++;
				}
				if (num4 > num)
				{
					num2 = num;
					num = num4;
				}
				else if (num4 > num2)
				{
					num2 = num4;
				}
			}
			return (float)Mathf.Min(num, num2);
		}

		private static bool NoRoofAroundAndWalkable(IntVec3 cell, Map map)
		{
			bool result;
			if (!cell.Walkable(map))
			{
				result = false;
			}
			else if (cell.Roofed(map))
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4(i);
					IntVec3 c = rot.FacingCell + cell;
					if (c.InBounds(map) && c.Roofed(map))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		private static float GetMountainousnessScoreAt(IntVec3 cell, Map map)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < 700; i += 10)
			{
				IntVec3 c = cell + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isNaturalRock)
					{
						num += 1f;
					}
					else if (c.Roofed(map) && c.GetRoof(map).isThickRoof)
					{
						num += 0.5f;
					}
					num2++;
				}
			}
			return num / (float)num2;
		}

		private static void CalculateTraversalDistancesToUnroofed(Map map)
		{
			InfestationCellFinder.tempUnroofedRegions.Clear();
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					Region region = intVec.GetRegion(map, RegionType.Set_Passable);
					if (region != null && InfestationCellFinder.NoRoofAroundAndWalkable(intVec, map))
					{
						InfestationCellFinder.tempUnroofedRegions.Add(region);
					}
				}
			}
			Dijkstra<Region>.Run(InfestationCellFinder.tempUnroofedRegions, (Region x) => x.Neighbors, (Region a, Region b) => Mathf.Sqrt((float)a.extentsClose.CenterCell.DistanceToSquared(b.extentsClose.CenterCell)), InfestationCellFinder.regionsDistanceToUnroofed, null);
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
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (InfestationCellFinder.closedAreaSize[j, i] == 0 && !intVec.Impassable(map))
					{
						int area = 0;
						map.floodFiller.FloodFill(intVec, (IntVec3 c) => !c.Impassable(map), delegate(IntVec3 c)
						{
							area++;
						}, int.MaxValue, false, null);
						area = Mathf.Min(area, 255);
						map.floodFiller.FloodFill(intVec, (IntVec3 c) => !c.Impassable(map), delegate(IntVec3 c)
						{
							InfestationCellFinder.closedAreaSize[c] = (byte)area;
						}, int.MaxValue, false, null);
					}
				}
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
			InfestationCellFinder.distToColonyBuilding.Clear(byte.MaxValue);
			InfestationCellFinder.tmpColonyBuildingsLocs.Clear();
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				InfestationCellFinder.tmpColonyBuildingsLocs.Add(allBuildingsColonist[i].Position);
			}
			Dijkstra<IntVec3>.Run(InfestationCellFinder.tmpColonyBuildingsLocs, (IntVec3 x) => DijkstraUtility.AdjacentCellsNeighborsGetter(x, map), delegate(IntVec3 a, IntVec3 b)
			{
				float result;
				if (a.x == b.x || a.z == b.z)
				{
					result = 1f;
				}
				else
				{
					result = 1.41421354f;
				}
				return result;
			}, InfestationCellFinder.tmpDistanceResult, null);
			for (int j = 0; j < InfestationCellFinder.tmpDistanceResult.Count; j++)
			{
				InfestationCellFinder.distToColonyBuilding[InfestationCellFinder.tmpDistanceResult[j].Key] = (byte)Mathf.Min(InfestationCellFinder.tmpDistanceResult[j].Value, 254.999f);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static InfestationCellFinder()
		{
		}

		[CompilerGenerated]
		private static float <TryFindCell>m__0(InfestationCellFinder.LocationCandidate x)
		{
			return x.score;
		}

		[CompilerGenerated]
		private static IEnumerable<Region> <CalculateTraversalDistancesToUnroofed>m__1(Region x)
		{
			return x.Neighbors;
		}

		[CompilerGenerated]
		private static float <CalculateTraversalDistancesToUnroofed>m__2(Region a, Region b)
		{
			return Mathf.Sqrt((float)a.extentsClose.CenterCell.DistanceToSquared(b.extentsClose.CenterCell));
		}

		[CompilerGenerated]
		private static float <CalculateDistanceToColonyBuildingGrid>m__3(IntVec3 a, IntVec3 b)
		{
			float result;
			if (a.x == b.x || a.z == b.z)
			{
				result = 1f;
			}
			else
			{
				result = 1.41421354f;
			}
			return result;
		}

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

		[CompilerGenerated]
		private sealed class <TryFindCell>c__AnonStorey0
		{
			internal Map map;

			public <TryFindCell>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return InfestationCellFinder.GetScoreAt(x, this.map) > 0f && x.GetFirstThing(this.map, ThingDefOf.Hive) == null && x.GetFirstThing(this.map, ThingDefOf.TunnelHiveSpawner) == null;
			}
		}

		[CompilerGenerated]
		private sealed class <CalculateClosedAreaSizeGrid>c__AnonStorey1
		{
			internal Map map;

			public <CalculateClosedAreaSizeGrid>c__AnonStorey1()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <CalculateClosedAreaSizeGrid>c__AnonStorey2
		{
			internal int area;

			internal InfestationCellFinder.<CalculateClosedAreaSizeGrid>c__AnonStorey1 <>f__ref$1;

			public <CalculateClosedAreaSizeGrid>c__AnonStorey2()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return !c.Impassable(this.<>f__ref$1.map);
			}

			internal void <>m__1(IntVec3 c)
			{
				this.area++;
			}

			internal bool <>m__2(IntVec3 c)
			{
				return !c.Impassable(this.<>f__ref$1.map);
			}

			internal void <>m__3(IntVec3 c)
			{
				InfestationCellFinder.closedAreaSize[c] = (byte)this.area;
			}
		}

		[CompilerGenerated]
		private sealed class <CalculateDistanceToColonyBuildingGrid>c__AnonStorey3
		{
			internal Map map;

			public <CalculateDistanceToColonyBuildingGrid>c__AnonStorey3()
			{
			}

			internal IEnumerable<IntVec3> <>m__0(IntVec3 x)
			{
				return DijkstraUtility.AdjacentCellsNeighborsGetter(x, this.map);
			}
		}
	}
}
