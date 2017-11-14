using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	public static class CellFinder
	{
		public static float EdgeRoadChance_Ignore = 0f;

		public static float EdgeRoadChance_Animal = 0f;

		public static float EdgeRoadChance_Hostile = 0.2f;

		public static float EdgeRoadChance_Neutral = 0.75f;

		public static float EdgeRoadChance_Friendly = 0.75f;

		public static float EdgeRoadChance_Always = 1f;

		private static List<IntVec3> workingCells = new List<IntVec3>();

		private static List<Region> workingRegions = new List<Region>();

		private static List<int> workingListX = new List<int>();

		private static List<int> workingListZ = new List<int>();

		private static List<IntVec3> mapEdgeCells;

		private static IntVec3 mapEdgeCellsSize;

		private static List<IntVec3>[] mapSingleEdgeCells = new List<IntVec3>[4];

		private static IntVec3 mapSingleEdgeCellsSize;

		private static Dictionary<IntVec3, float> tmpDistances = new Dictionary<IntVec3, float>();

		private static Dictionary<IntVec3, IntVec3> tmpParents = new Dictionary<IntVec3, IntVec3>();

		private static List<IntVec3> tmpCells = new List<IntVec3>();

		public static IntVec3 RandomCell(Map map)
		{
			IntVec3 size = map.Size;
			int newX = Rand.Range(0, size.x);
			IntVec3 size2 = map.Size;
			return new IntVec3(newX, 0, Rand.Range(0, size2.z));
		}

		public static IntVec3 RandomEdgeCell(Map map)
		{
			IntVec3 result = default(IntVec3);
			if (Rand.Value < 0.5)
			{
				if (Rand.Value < 0.5)
				{
					result.x = 0;
				}
				else
				{
					IntVec3 size = map.Size;
					result.x = size.x - 1;
				}
				IntVec3 size2 = map.Size;
				result.z = Rand.Range(0, size2.z);
			}
			else
			{
				if (Rand.Value < 0.5)
				{
					result.z = 0;
				}
				else
				{
					IntVec3 size3 = map.Size;
					result.z = size3.z - 1;
				}
				IntVec3 size4 = map.Size;
				result.x = Rand.Range(0, size4.x);
			}
			return result;
		}

		public static IntVec3 RandomEdgeCell(Rot4 dir, Map map)
		{
			if (dir == Rot4.North)
			{
				IntVec3 size = map.Size;
				int newX = Rand.Range(0, size.x);
				IntVec3 size2 = map.Size;
				return new IntVec3(newX, 0, size2.z - 1);
			}
			if (dir == Rot4.South)
			{
				IntVec3 size3 = map.Size;
				return new IntVec3(Rand.Range(0, size3.x), 0, 0);
			}
			if (dir == Rot4.West)
			{
				IntVec3 size4 = map.Size;
				return new IntVec3(0, 0, Rand.Range(0, size4.z));
			}
			if (dir == Rot4.East)
			{
				IntVec3 size5 = map.Size;
				int newX2 = size5.x - 1;
				IntVec3 size6 = map.Size;
				return new IntVec3(newX2, 0, Rand.Range(0, size6.z));
			}
			return IntVec3.Invalid;
		}

		public static IntVec3 RandomNotEdgeCell(int minEdgeDistance, Map map)
		{
			IntVec3 size = map.Size;
			if (minEdgeDistance <= size.x / 2)
			{
				IntVec3 size2 = map.Size;
				if (minEdgeDistance > size2.z / 2)
					goto IL_002c;
				IntVec3 size3 = map.Size;
				int newX = Rand.Range(minEdgeDistance, size3.x - minEdgeDistance);
				IntVec3 size4 = map.Size;
				int newZ = Rand.Range(minEdgeDistance, size4.z - minEdgeDistance);
				return new IntVec3(newX, 0, newZ);
			}
			goto IL_002c;
			IL_002c:
			return IntVec3.Invalid;
		}

		public static bool TryFindClosestRegionWith(Region rootReg, TraverseParms traverseParms, Predicate<Region> validator, int maxRegions, out Region result, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (rootReg == null)
			{
				result = null;
				return false;
			}
			Region localResult = null;
			RegionTraverser.BreadthFirstTraverse(rootReg, (Region from, Region r) => r.Allows(traverseParms, true), delegate(Region r)
			{
				if (validator(r))
				{
					localResult = r;
					return true;
				}
				return false;
			}, maxRegions, traversableRegionTypes);
			result = localResult;
			return result != null;
		}

		public static Region RandomRegionNear(Region root, int maxRegions, TraverseParms traverseParms, Predicate<Region> validator = null, Pawn pawnToAllow = null, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			if (maxRegions <= 1)
			{
				return root;
			}
			CellFinder.workingRegions.Clear();
			RegionTraverser.BreadthFirstTraverse(root, delegate(Region from, Region r)
			{
				int result2;
				if ((validator == null || validator(r)) && r.Allows(traverseParms, true))
				{
					result2 = ((pawnToAllow == null || !r.IsForbiddenEntirely(pawnToAllow)) ? 1 : 0);
					goto IL_004e;
				}
				result2 = 0;
				goto IL_004e;
				IL_004e:
				return (byte)result2 != 0;
			}, delegate(Region r)
			{
				CellFinder.workingRegions.Add(r);
				return false;
			}, maxRegions, traversableRegionTypes);
			Region result = CellFinder.workingRegions.RandomElementByWeight((Region r) => (float)r.CellCount);
			CellFinder.workingRegions.Clear();
			return result;
		}

		public static void AllRegionsNear(List<Region> results, Region root, int maxRegions, TraverseParms traverseParms, Predicate<Region> validator = null, Pawn pawnToAllow = null, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (results == null)
			{
				Log.ErrorOnce("Attempted to call AllRegionsNear with an invalid results list", 60733193);
			}
			else
			{
				results.Clear();
				if (root == null)
				{
					Log.ErrorOnce("Attempted to call AllRegionsNear with an invalid root", 9107839);
				}
				else
				{
					RegionTraverser.BreadthFirstTraverse(root, delegate(Region from, Region r)
					{
						int result;
						if ((validator == null || validator(r)) && r.Allows(traverseParms, true))
						{
							result = ((pawnToAllow == null || !r.IsForbiddenEntirely(pawnToAllow)) ? 1 : 0);
							goto IL_004e;
						}
						result = 0;
						goto IL_004e;
						IL_004e:
						return (byte)result != 0;
					}, delegate(Region r)
					{
						results.Add(r);
						return false;
					}, maxRegions, traversableRegionTypes);
				}
			}
		}

		public static bool TryFindRandomReachableCellNear(IntVec3 root, Map map, float radius, TraverseParms traverseParms, Predicate<IntVec3> cellValidator, Predicate<Region> regionValidator, out IntVec3 result, int maxRegions = 999999)
		{
			if (map == null)
			{
				Log.ErrorOnce("Tried to find reachable cell in a null map", 61037855);
				result = IntVec3.Invalid;
				return false;
			}
			Region region = root.GetRegion(map, RegionType.Set_Passable);
			if (region == null)
			{
				result = IntVec3.Invalid;
				return false;
			}
			CellFinder.workingRegions.Clear();
			float radSquared = radius * radius;
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(traverseParms, true) && (radius > 1000.0 || r.extentsClose.ClosestDistSquaredTo(root) <= radSquared) && (regionValidator == null || regionValidator(r)), delegate(Region r)
			{
				CellFinder.workingRegions.Add(r);
				return false;
			}, maxRegions, RegionType.Set_Passable);
			while (CellFinder.workingRegions.Count > 0)
			{
				Region region2 = CellFinder.workingRegions.RandomElementByWeight((Region r) => (float)r.CellCount);
				if (region2.TryFindRandomCellInRegion((Predicate<IntVec3>)((IntVec3 c) => (float)(c - root).LengthHorizontalSquared <= radSquared && cellValidator(c)), out result))
				{
					CellFinder.workingRegions.Clear();
					return true;
				}
				CellFinder.workingRegions.Remove(region2);
			}
			result = IntVec3.Invalid;
			CellFinder.workingRegions.Clear();
			return false;
		}

		public static IntVec3 RandomClosewalkCellNear(IntVec3 root, Map map, int radius, Predicate<IntVec3> extraValidator = null)
		{
			IntVec3 result = default(IntVec3);
			if (CellFinder.TryRandomClosewalkCellNear(root, map, radius, out result, extraValidator))
			{
				return result;
			}
			return root;
		}

		public static bool TryRandomClosewalkCellNear(IntVec3 root, Map map, int radius, out IntVec3 result, Predicate<IntVec3> extraValidator = null)
		{
			return CellFinder.TryFindRandomReachableCellNear(root, map, (float)radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (Predicate<IntVec3>)((IntVec3 c) => c.Standable(map) && (extraValidator == null || extraValidator(c))), (Predicate<Region>)null, out result, 999999);
		}

		public static IntVec3 RandomClosewalkCellNearNotForbidden(IntVec3 root, Map map, int radius, Pawn pawn)
		{
			IntVec3 result = default(IntVec3);
			if (!CellFinder.TryFindRandomReachableCellNear(root, map, (float)radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (Predicate<IntVec3>)((IntVec3 c) => !c.IsForbidden(pawn) && c.Standable(map)), (Predicate<Region>)null, out result, 999999))
			{
				return CellFinder.RandomClosewalkCellNear(root, map, radius, null);
			}
			return result;
		}

		public static bool TryFindRandomCellInRegion(this Region reg, Predicate<IntVec3> validator, out IntVec3 result)
		{
			int num = 0;
			while (num < 10)
			{
				result = reg.RandomCell;
				if (validator != null && !validator(result))
				{
					num++;
					continue;
				}
				return true;
			}
			CellFinder.workingCells.Clear();
			CellFinder.workingCells.AddRange(reg.Cells);
			CellFinder.workingCells.Shuffle();
			int num2 = 0;
			while (num2 < CellFinder.workingCells.Count)
			{
				result = CellFinder.workingCells[num2];
				if (validator != null && !validator(result))
				{
					num2++;
					continue;
				}
				return true;
			}
			result = reg.RandomCell;
			return false;
		}

		public static bool TryFindRandomCellNear(IntVec3 root, Map map, int squareRadius, Predicate<IntVec3> validator, out IntVec3 result)
		{
			int num = root.x - squareRadius;
			int num2 = root.x + squareRadius;
			int num3 = root.z - squareRadius;
			int num4 = root.z + squareRadius;
			if (num < 0)
			{
				num = 0;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			int num5 = num2;
			IntVec3 size = map.Size;
			if (num5 > size.x)
			{
				IntVec3 size2 = map.Size;
				num2 = size2.x;
			}
			int num6 = num4;
			IntVec3 size3 = map.Size;
			if (num6 > size3.z)
			{
				IntVec3 size4 = map.Size;
				num4 = size4.z;
			}
			int num7 = 0;
			while (true)
			{
				IntVec3 intVec = new IntVec3(Rand.RangeInclusive(num, num2), 0, Rand.RangeInclusive(num3, num4));
				if (validator != null && !validator(intVec))
				{
					if (DebugViewSettings.drawDestSearch)
					{
						map.debugDrawer.FlashCell(intVec, 0f, "inv", 50);
					}
					num7++;
					if (num7 > 20)
						break;
					continue;
				}
				if (DebugViewSettings.drawDestSearch)
				{
					map.debugDrawer.FlashCell(intVec, 0.5f, "found", 50);
				}
				result = intVec;
				return true;
			}
			CellFinder.workingListX.Clear();
			CellFinder.workingListZ.Clear();
			for (int i = num; i <= num2; i++)
			{
				CellFinder.workingListX.Add(i);
			}
			for (int j = num3; j <= num4; j++)
			{
				CellFinder.workingListZ.Add(j);
			}
			CellFinder.workingListX.Shuffle();
			CellFinder.workingListZ.Shuffle();
			for (int k = 0; k < CellFinder.workingListX.Count; k++)
			{
				for (int l = 0; l < CellFinder.workingListZ.Count; l++)
				{
					IntVec3 intVec = new IntVec3(CellFinder.workingListX[k], 0, CellFinder.workingListZ[l]);
					if (validator(intVec))
					{
						if (DebugViewSettings.drawDestSearch)
						{
							map.debugDrawer.FlashCell(intVec, 0.6f, "found2", 50);
						}
						result = intVec;
						return true;
					}
					if (DebugViewSettings.drawDestSearch)
					{
						map.debugDrawer.FlashCell(intVec, 0.25f, "inv2", 50);
					}
				}
			}
			result = root;
			return false;
		}

		public static bool TryFindRandomPawnExitCell(Pawn searcher, out IntVec3 result)
		{
			return CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 c) => !searcher.Map.roofGrid.Roofed(c) && c.Walkable(searcher.Map) && searcher.CanReach(c, PathEndMode.OnCell, Danger.Some, false, TraverseMode.ByPawn)), searcher.Map, 0f, out result);
		}

		public static bool TryFindRandomEdgeCellWith(Predicate<IntVec3> validator, Map map, float roadChance, out IntVec3 result)
		{
			if (Rand.Chance(roadChance))
			{
				bool flag = (from c in map.roadInfo.roadEdgeTiles
				where validator(c)
				select c).TryRandomElement<IntVec3>(out result);
				if (flag)
				{
					return flag;
				}
			}
			for (int i = 0; i < 100; i++)
			{
				result = CellFinder.RandomEdgeCell(map);
				if (validator(result))
				{
					return true;
				}
			}
			if (CellFinder.mapEdgeCells == null || map.Size != CellFinder.mapEdgeCellsSize)
			{
				CellFinder.mapEdgeCellsSize = map.Size;
				CellFinder.mapEdgeCells = new List<IntVec3>();
				foreach (IntVec3 edgeCell in CellRect.WholeMap(map).EdgeCells)
				{
					CellFinder.mapEdgeCells.Add(edgeCell);
				}
			}
			CellFinder.mapEdgeCells.Shuffle();
			for (int j = 0; j < CellFinder.mapEdgeCells.Count; j++)
			{
				try
				{
					if (validator(CellFinder.mapEdgeCells[j]))
					{
						result = CellFinder.mapEdgeCells[j];
						return true;
					}
				}
				catch (Exception ex)
				{
					Log.Error("TryFindRandomEdgeCellWith exception validating " + CellFinder.mapEdgeCells[j] + ": " + ex.ToString());
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		public static bool TryFindRandomEdgeCellWith(Predicate<IntVec3> validator, Map map, Rot4 dir, float roadChance, out IntVec3 result)
		{
			if (Rand.Value < roadChance)
			{
				bool flag = (from c in map.roadInfo.roadEdgeTiles
				where validator(c) && c.OnEdge(map, dir)
				select c).TryRandomElement<IntVec3>(out result);
				if (flag)
				{
					return flag;
				}
			}
			for (int i = 0; i < 100; i++)
			{
				result = CellFinder.RandomEdgeCell(dir, map);
				if (validator(result))
				{
					return true;
				}
			}
			int asInt = dir.AsInt;
			if (CellFinder.mapSingleEdgeCells[asInt] == null || map.Size != CellFinder.mapSingleEdgeCellsSize)
			{
				CellFinder.mapSingleEdgeCellsSize = map.Size;
				CellFinder.mapSingleEdgeCells[asInt] = new List<IntVec3>();
				foreach (IntVec3 edgeCell in CellRect.WholeMap(map).GetEdgeCells(dir))
				{
					CellFinder.mapSingleEdgeCells[asInt].Add(edgeCell);
				}
			}
			List<IntVec3> list = CellFinder.mapSingleEdgeCells[asInt];
			list.Shuffle();
			int j = 0;
			int count = list.Count;
			for (; j < count; j++)
			{
				try
				{
					if (validator(list[j]))
					{
						result = list[j];
						return true;
					}
				}
				catch (Exception ex)
				{
					Log.Error("TryFindRandomEdgeCellWith exception validating " + list[j] + ": " + ex.ToString());
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		public static bool TryFindRandomEdgeCellNearWith(IntVec3 near, float radius, Map map, Predicate<IntVec3> validator, out IntVec3 spot)
		{
			CellRect cellRect = CellRect.CenteredOn(near, Mathf.CeilToInt(radius));
			Predicate<IntVec3> predicate = (IntVec3 x) => x.InHorDistOf(near, radius) && x.OnEdge(map) && validator(x);
			if (CellRect.WholeMap(map).EdgeCellsCount < cellRect.Area)
			{
				return CellFinder.TryFindRandomEdgeCellWith(predicate, map, CellFinder.EdgeRoadChance_Ignore, out spot);
			}
			return CellFinder.TryFindRandomCellInsideWith(cellRect, predicate, out spot);
		}

		public static bool TryFindBestPawnStandCell(Pawn forPawn, out IntVec3 cell, bool cellByCell = false)
		{
			cell = IntVec3.Invalid;
			int num = -1;
			float radius = 10f;
			while (true)
			{
				CellFinder.tmpDistances.Clear();
				CellFinder.tmpParents.Clear();
				Dijkstra<IntVec3>.Run(forPawn.Position, (IntVec3 x) => CellFinder.GetAdjacentCardinalCellsForBestStandCell(x, radius, forPawn), delegate(IntVec3 from, IntVec3 to)
				{
					float num6 = 1f;
					if (from.x != to.x && from.z != to.z)
					{
						num6 = 1.41421354f;
					}
					if (!to.Standable(forPawn.Map))
					{
						num6 = (float)(num6 + 3.0);
					}
					if (PawnUtility.AnyPawnBlockingPathAt(to, forPawn, false, false))
					{
						num6 = (float)((to.GetThingList(forPawn.Map).Find((Thing x) => x is Pawn && x.HostileTo(forPawn)) == null) ? (num6 + 15.0) : (num6 + 40.0));
					}
					Building_Door building_Door = to.GetEdifice(forPawn.Map) as Building_Door;
					if (building_Door != null && !building_Door.FreePassage)
					{
						num6 = (float)((!building_Door.PawnCanOpen(forPawn)) ? (num6 + 50.0) : (num6 + 6.0));
					}
					return num6;
				}, CellFinder.tmpDistances, CellFinder.tmpParents);
				if (CellFinder.tmpDistances.Count == num)
				{
					return false;
				}
				float num2 = 0f;
				foreach (KeyValuePair<IntVec3, float> tmpDistance in CellFinder.tmpDistances)
				{
					if ((!cell.IsValid || !(tmpDistance.Value >= num2)) && tmpDistance.Key.Walkable(forPawn.Map) && !PawnUtility.AnyPawnBlockingPathAt(tmpDistance.Key, forPawn, false, false))
					{
						Building_Door door = tmpDistance.Key.GetDoor(forPawn.Map);
						if (door == null || door.FreePassage)
						{
							cell = tmpDistance.Key;
							num2 = tmpDistance.Value;
						}
					}
				}
				if (cell.IsValid)
				{
					if (!cellByCell)
					{
						return true;
					}
					IntVec3 intVec = cell;
					int num3 = 0;
					while (intVec.IsValid && intVec != forPawn.Position)
					{
						num3++;
						if (num3 >= 10000)
						{
							Log.Error("Too many iterations.");
							break;
						}
						if (intVec.Walkable(forPawn.Map))
						{
							Building_Door door2 = intVec.GetDoor(forPawn.Map);
							if (door2 == null || door2.FreePassage)
							{
								cell = intVec;
							}
						}
						intVec = CellFinder.tmpParents[intVec];
					}
					return true;
				}
				float num4 = radius;
				IntVec3 size = forPawn.Map.Size;
				if (num4 > (float)size.x)
				{
					float num5 = radius;
					IntVec3 size2 = forPawn.Map.Size;
					if (num5 > (float)size2.z)
						break;
				}
				radius = (float)(radius * 2.0);
				num = CellFinder.tmpDistances.Count;
			}
			return false;
		}

		public static bool TryFindRandomCellInsideWith(CellRect cellRect, Predicate<IntVec3> predicate, out IntVec3 result)
		{
			int area = cellRect.Area;
			int num = Mathf.Max(Mathf.RoundToInt(Mathf.Sqrt((float)area)), 5);
			for (int i = 0; i < num; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				if (predicate(randomCell))
				{
					result = randomCell;
					return true;
				}
			}
			CellFinder.tmpCells.Clear();
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			while (!iterator.Done())
			{
				CellFinder.tmpCells.Add(iterator.Current);
				iterator.MoveNext();
			}
			CellFinder.tmpCells.Shuffle();
			int j = 0;
			int count = CellFinder.tmpCells.Count;
			for (; j < count; j++)
			{
				if (predicate(CellFinder.tmpCells[j]))
				{
					result = CellFinder.tmpCells[j];
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		public static IntVec3 RandomSpawnCellForPawnNear(IntVec3 root, Map map, int firstTryWithRadius = 4)
		{
			IntVec3 result = default(IntVec3);
			if (CellFinder.TryFindRandomSpawnCellForPawnNear(root, map, out result, firstTryWithRadius))
			{
				return result;
			}
			return root;
		}

		public static bool TryFindRandomSpawnCellForPawnNear(IntVec3 root, Map map, out IntVec3 result, int firstTryWithRadius = 4)
		{
			if (root.Standable(map) && root.GetFirstPawn(map) == null)
			{
				result = root;
				return true;
			}
			bool rootFogged = root.Fogged(map);
			int num = firstTryWithRadius;
			for (int i = 0; i < 3; i++)
			{
				if (CellFinder.TryFindRandomReachableCellNear(root, map, (float)num, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (Predicate<IntVec3>)((IntVec3 c) => c.Standable(map) && (rootFogged || !c.Fogged(map)) && c.GetFirstPawn(map) == null), (Predicate<Region>)null, out result, 999999))
				{
					return true;
				}
				num *= 2;
			}
			num = firstTryWithRadius + 1;
			while (true)
			{
				if (CellFinder.TryRandomClosewalkCellNear(root, map, num, out result, (Predicate<IntVec3>)null))
				{
					return true;
				}
				int num2 = num;
				IntVec3 size = map.Size;
				if (num2 > size.x / 2)
				{
					int num3 = num;
					IntVec3 size2 = map.Size;
					if (num3 > size2.z / 2)
						break;
				}
				num *= 2;
			}
			result = root;
			return false;
		}

		private static IEnumerable<IntVec3> GetAdjacentCardinalCellsForBestStandCell(IntVec3 x, float radius, Pawn pawn)
		{
			if (!((float)(x - pawn.Position).LengthManhattan > radius))
			{
				int i = 0;
				IntVec3 c;
				while (true)
				{
					if (i < 4)
					{
						c = x + GenAdj.CardinalDirections[i];
						if (c.InBounds(pawn.Map) && c.Walkable(pawn.Map))
						{
							Building_Door door = c.GetEdifice(pawn.Map) as Building_Door;
							if (door == null)
								break;
							if (door.CanPhysicallyPass(pawn))
								break;
						}
						i++;
						continue;
					}
					yield break;
				}
				yield return c;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
