using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000F2A RID: 3882
	public static class CellFinder
	{
		// Token: 0x06005D37 RID: 23863 RVA: 0x002F2C64 File Offset: 0x002F1064
		public static IntVec3 RandomCell(Map map)
		{
			return new IntVec3(Rand.Range(0, map.Size.x), 0, Rand.Range(0, map.Size.z));
		}

		// Token: 0x06005D38 RID: 23864 RVA: 0x002F2CA8 File Offset: 0x002F10A8
		public static IntVec3 RandomEdgeCell(Map map)
		{
			IntVec3 result = default(IntVec3);
			if (Rand.Value < 0.5f)
			{
				if (Rand.Value < 0.5f)
				{
					result.x = 0;
				}
				else
				{
					result.x = map.Size.x - 1;
				}
				result.z = Rand.Range(0, map.Size.z);
			}
			else
			{
				if (Rand.Value < 0.5f)
				{
					result.z = 0;
				}
				else
				{
					result.z = map.Size.z - 1;
				}
				result.x = Rand.Range(0, map.Size.x);
			}
			return result;
		}

		// Token: 0x06005D39 RID: 23865 RVA: 0x002F2D80 File Offset: 0x002F1180
		public static IntVec3 RandomEdgeCell(Rot4 dir, Map map)
		{
			IntVec3 result;
			if (dir == Rot4.North)
			{
				result = new IntVec3(Rand.Range(0, map.Size.x), 0, map.Size.z - 1);
			}
			else if (dir == Rot4.South)
			{
				result = new IntVec3(Rand.Range(0, map.Size.x), 0, 0);
			}
			else if (dir == Rot4.West)
			{
				result = new IntVec3(0, 0, Rand.Range(0, map.Size.z));
			}
			else if (dir == Rot4.East)
			{
				result = new IntVec3(map.Size.x - 1, 0, Rand.Range(0, map.Size.z));
			}
			else
			{
				result = IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x06005D3A RID: 23866 RVA: 0x002F2E80 File Offset: 0x002F1280
		public static IntVec3 RandomNotEdgeCell(int minEdgeDistance, Map map)
		{
			IntVec3 result;
			if (minEdgeDistance > map.Size.x / 2 || minEdgeDistance > map.Size.z / 2)
			{
				result = IntVec3.Invalid;
			}
			else
			{
				int newX = Rand.Range(minEdgeDistance, map.Size.x - minEdgeDistance);
				int newZ = Rand.Range(minEdgeDistance, map.Size.z - minEdgeDistance);
				result = new IntVec3(newX, 0, newZ);
			}
			return result;
		}

		// Token: 0x06005D3B RID: 23867 RVA: 0x002F2F08 File Offset: 0x002F1308
		public static bool TryFindClosestRegionWith(Region rootReg, TraverseParms traverseParms, Predicate<Region> validator, int maxRegions, out Region result, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			bool result2;
			if (rootReg == null)
			{
				result = null;
				result2 = false;
			}
			else
			{
				Region localResult = null;
				RegionTraverser.BreadthFirstTraverse(rootReg, (Region from, Region r) => r.Allows(traverseParms, true), delegate(Region r)
				{
					bool result3;
					if (validator(r))
					{
						localResult = r;
						result3 = true;
					}
					else
					{
						result3 = false;
					}
					return result3;
				}, maxRegions, traversableRegionTypes);
				result = localResult;
				result2 = (result != null);
			}
			return result2;
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x002F2F80 File Offset: 0x002F1380
		public static Region RandomRegionNear(Region root, int maxRegions, TraverseParms traverseParms, Predicate<Region> validator = null, Pawn pawnToAllow = null, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			Region result;
			if (maxRegions <= 1)
			{
				result = root;
			}
			else
			{
				CellFinder.workingRegions.Clear();
				RegionTraverser.BreadthFirstTraverse(root, (Region from, Region r) => (validator == null || validator(r)) && r.Allows(traverseParms, true) && (pawnToAllow == null || !r.IsForbiddenEntirely(pawnToAllow)), delegate(Region r)
				{
					CellFinder.workingRegions.Add(r);
					return false;
				}, maxRegions, traversableRegionTypes);
				Region region = CellFinder.workingRegions.RandomElementByWeight((Region r) => (float)r.CellCount);
				CellFinder.workingRegions.Clear();
				result = region;
			}
			return result;
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x002F3040 File Offset: 0x002F1440
		public static void AllRegionsNear(List<Region> results, Region root, int maxRegions, TraverseParms traverseParms, Predicate<Region> validator = null, Pawn pawnToAllow = null, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (results == null)
			{
				Log.ErrorOnce("Attempted to call AllRegionsNear with an invalid results list", 60733193, false);
			}
			else
			{
				results.Clear();
				if (root == null)
				{
					Log.ErrorOnce("Attempted to call AllRegionsNear with an invalid root", 9107839, false);
				}
				else
				{
					RegionTraverser.BreadthFirstTraverse(root, (Region from, Region r) => (validator == null || validator(r)) && r.Allows(traverseParms, true) && (pawnToAllow == null || !r.IsForbiddenEntirely(pawnToAllow)), delegate(Region r)
					{
						results.Add(r);
						return false;
					}, maxRegions, traversableRegionTypes);
				}
			}
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x002F30DC File Offset: 0x002F14DC
		public static bool TryFindRandomReachableCellNear(IntVec3 root, Map map, float radius, TraverseParms traverseParms, Predicate<IntVec3> cellValidator, Predicate<Region> regionValidator, out IntVec3 result, int maxRegions = 999999)
		{
			bool result2;
			if (map == null)
			{
				Log.ErrorOnce("Tried to find reachable cell in a null map", 61037855, false);
				result = IntVec3.Invalid;
				result2 = false;
			}
			else
			{
				Region region = root.GetRegion(map, RegionType.Set_Passable);
				if (region == null)
				{
					result = IntVec3.Invalid;
					result2 = false;
				}
				else
				{
					CellFinder.workingRegions.Clear();
					float radSquared = radius * radius;
					RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(traverseParms, true) && (radius > 1000f || r.extentsClose.ClosestDistSquaredTo(root) <= radSquared) && (regionValidator == null || regionValidator(r)), delegate(Region r)
					{
						CellFinder.workingRegions.Add(r);
						return false;
					}, maxRegions, RegionType.Set_Passable);
					while (CellFinder.workingRegions.Count > 0)
					{
						Region region2 = CellFinder.workingRegions.RandomElementByWeight((Region r) => (float)r.CellCount);
						if (region2.TryFindRandomCellInRegion((IntVec3 c) => (float)(c - root).LengthHorizontalSquared <= radSquared && (cellValidator == null || cellValidator(c)), out result))
						{
							CellFinder.workingRegions.Clear();
							return true;
						}
						CellFinder.workingRegions.Remove(region2);
					}
					result = IntVec3.Invalid;
					CellFinder.workingRegions.Clear();
					result2 = false;
				}
			}
			return result2;
		}

		// Token: 0x06005D3F RID: 23871 RVA: 0x002F324C File Offset: 0x002F164C
		public static IntVec3 RandomClosewalkCellNear(IntVec3 root, Map map, int radius, Predicate<IntVec3> extraValidator = null)
		{
			IntVec3 intVec;
			IntVec3 result;
			if (CellFinder.TryRandomClosewalkCellNear(root, map, radius, out intVec, extraValidator))
			{
				result = intVec;
			}
			else
			{
				result = root;
			}
			return result;
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x002F327C File Offset: 0x002F167C
		public static bool TryRandomClosewalkCellNear(IntVec3 root, Map map, int radius, out IntVec3 result, Predicate<IntVec3> extraValidator = null)
		{
			return CellFinder.TryFindRandomReachableCellNear(root, map, (float)radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 c) => c.Standable(map) && (extraValidator == null || extraValidator(c)), null, out result, 999999);
		}

		// Token: 0x06005D41 RID: 23873 RVA: 0x002F32D0 File Offset: 0x002F16D0
		public static IntVec3 RandomClosewalkCellNearNotForbidden(IntVec3 root, Map map, int radius, Pawn pawn)
		{
			IntVec3 intVec;
			IntVec3 result;
			if (!CellFinder.TryFindRandomReachableCellNear(root, map, (float)radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 c) => !c.IsForbidden(pawn) && c.Standable(map), null, out intVec, 999999))
			{
				result = CellFinder.RandomClosewalkCellNear(root, map, radius, null);
			}
			else
			{
				result = intVec;
			}
			return result;
		}

		// Token: 0x06005D42 RID: 23874 RVA: 0x002F3340 File Offset: 0x002F1740
		public static bool TryFindRandomCellInRegion(this Region reg, Predicate<IntVec3> validator, out IntVec3 result)
		{
			for (int i = 0; i < 10; i++)
			{
				result = reg.RandomCell;
				if (validator == null || validator(result))
				{
					return true;
				}
			}
			CellFinder.workingCells.Clear();
			CellFinder.workingCells.AddRange(reg.Cells);
			CellFinder.workingCells.Shuffle<IntVec3>();
			for (int j = 0; j < CellFinder.workingCells.Count; j++)
			{
				result = CellFinder.workingCells[j];
				if (validator == null || validator(result))
				{
					return true;
				}
			}
			result = reg.RandomCell;
			return false;
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x002F3414 File Offset: 0x002F1814
		public static bool TryFindRandomCellNear(IntVec3 root, Map map, int squareRadius, Predicate<IntVec3> validator, out IntVec3 result, int maxTries = -1)
		{
			int num = root.x - squareRadius;
			int num2 = root.x + squareRadius;
			int num3 = root.z - squareRadius;
			int num4 = root.z + squareRadius;
			int num5 = (num2 - num + 1) * (num4 - num3 + 1);
			if (num < 0)
			{
				num = 0;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num2 > map.Size.x)
			{
				num2 = map.Size.x;
			}
			if (num4 > map.Size.z)
			{
				num4 = map.Size.z;
			}
			int num6;
			bool flag;
			if (maxTries < 0 || maxTries >= num5)
			{
				num6 = 20;
				flag = false;
			}
			else
			{
				num6 = maxTries;
				flag = true;
			}
			for (int i = 0; i < num6; i++)
			{
				IntVec3 intVec = new IntVec3(Rand.RangeInclusive(num, num2), 0, Rand.RangeInclusive(num3, num4));
				if (validator == null || validator(intVec))
				{
					if (DebugViewSettings.drawDestSearch)
					{
						map.debugDrawer.FlashCell(intVec, 0.5f, "found", 50);
					}
					result = intVec;
					return true;
				}
				if (DebugViewSettings.drawDestSearch)
				{
					map.debugDrawer.FlashCell(intVec, 0f, "inv", 50);
				}
			}
			if (flag)
			{
				result = root;
				return false;
			}
			CellFinder.workingListX.Clear();
			CellFinder.workingListZ.Clear();
			for (int j = num; j <= num2; j++)
			{
				CellFinder.workingListX.Add(j);
			}
			for (int k = num3; k <= num4; k++)
			{
				CellFinder.workingListZ.Add(k);
			}
			CellFinder.workingListX.Shuffle<int>();
			CellFinder.workingListZ.Shuffle<int>();
			for (int l = 0; l < CellFinder.workingListX.Count; l++)
			{
				for (int m = 0; m < CellFinder.workingListZ.Count; m++)
				{
					IntVec3 intVec = new IntVec3(CellFinder.workingListX[l], 0, CellFinder.workingListZ[m]);
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

		// Token: 0x06005D44 RID: 23876 RVA: 0x002F36D8 File Offset: 0x002F1AD8
		public static bool TryFindRandomPawnExitCell(Pawn searcher, out IntVec3 result)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => !searcher.Map.roofGrid.Roofed(c) && c.Walkable(searcher.Map) && searcher.CanReach(c, PathEndMode.OnCell, Danger.Some, false, TraverseMode.ByPawn), searcher.Map, 0f, out result);
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x002F371C File Offset: 0x002F1B1C
		public static bool TryFindRandomEdgeCellWith(Predicate<IntVec3> validator, Map map, float roadChance, out IntVec3 result)
		{
			if (Rand.Chance(roadChance))
			{
				bool flag = (from c in map.roadInfo.roadEdgeTiles
				where validator(c)
				select c).TryRandomElement(out result);
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
				foreach (IntVec3 item in CellRect.WholeMap(map).EdgeCells)
				{
					CellFinder.mapEdgeCells.Add(item);
				}
			}
			CellFinder.mapEdgeCells.Shuffle<IntVec3>();
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
					Log.Error(string.Concat(new object[]
					{
						"TryFindRandomEdgeCellWith exception validating ",
						CellFinder.mapEdgeCells[j],
						": ",
						ex.ToString()
					}), false);
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x002F391C File Offset: 0x002F1D1C
		public static bool TryFindRandomEdgeCellWith(Predicate<IntVec3> validator, Map map, Rot4 dir, float roadChance, out IntVec3 result)
		{
			if (Rand.Value < roadChance)
			{
				bool flag = (from c in map.roadInfo.roadEdgeTiles
				where validator(c) && c.OnEdge(map, dir)
				select c).TryRandomElement(out result);
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
				foreach (IntVec3 item in CellRect.WholeMap(map).GetEdgeCells(dir))
				{
					CellFinder.mapSingleEdgeCells[asInt].Add(item);
				}
			}
			List<IntVec3> list = CellFinder.mapSingleEdgeCells[asInt];
			list.Shuffle<IntVec3>();
			int j = 0;
			int count = list.Count;
			while (j < count)
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
					Log.Error(string.Concat(new object[]
					{
						"TryFindRandomEdgeCellWith exception validating ",
						list[j],
						": ",
						ex.ToString()
					}), false);
				}
				j++;
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x002F3B68 File Offset: 0x002F1F68
		public static bool TryFindRandomEdgeCellNearWith(IntVec3 near, float radius, Map map, Predicate<IntVec3> validator, out IntVec3 spot)
		{
			CellRect cellRect = CellRect.CenteredOn(near, Mathf.CeilToInt(radius));
			Predicate<IntVec3> predicate = (IntVec3 x) => x.InHorDistOf(near, radius) && x.OnEdge(map) && validator(x);
			bool result;
			if (CellRect.WholeMap(map).EdgeCellsCount < cellRect.Area)
			{
				result = CellFinder.TryFindRandomEdgeCellWith(predicate, map, CellFinder.EdgeRoadChance_Ignore, out spot);
			}
			else
			{
				result = CellFinder.TryFindRandomCellInsideWith(cellRect, predicate, out spot);
			}
			return result;
		}

		// Token: 0x06005D48 RID: 23880 RVA: 0x002F3C08 File Offset: 0x002F2008
		public static bool TryFindBestPawnStandCell(Pawn forPawn, out IntVec3 cell, bool cellByCell = false)
		{
			cell = IntVec3.Invalid;
			int num = -1;
			float radius = 10f;
			for (;;)
			{
				CellFinder.tmpDistances.Clear();
				CellFinder.tmpParents.Clear();
				Dijkstra<IntVec3>.Run(forPawn.Position, (IntVec3 x) => CellFinder.GetAdjacentCardinalCellsForBestStandCell(x, radius, forPawn), delegate(IntVec3 from, IntVec3 to)
				{
					float num4 = 1f;
					if (from.x != to.x && from.z != to.z)
					{
						num4 = 1.41421354f;
					}
					if (!to.Standable(forPawn.Map))
					{
						num4 += 3f;
					}
					if (PawnUtility.AnyPawnBlockingPathAt(to, forPawn, false, false, false))
					{
						bool flag = to.GetThingList(forPawn.Map).Find((Thing x) => x is Pawn && x.HostileTo(forPawn)) != null;
						if (flag)
						{
							num4 += 40f;
						}
						else
						{
							num4 += 15f;
						}
					}
					Building_Door building_Door = to.GetEdifice(forPawn.Map) as Building_Door;
					if (building_Door != null && !building_Door.FreePassage)
					{
						if (building_Door.PawnCanOpen(forPawn))
						{
							num4 += 6f;
						}
						else
						{
							num4 += 50f;
						}
					}
					return num4;
				}, CellFinder.tmpDistances, CellFinder.tmpParents);
				if (CellFinder.tmpDistances.Count == num)
				{
					break;
				}
				float num2 = 0f;
				foreach (KeyValuePair<IntVec3, float> keyValuePair in CellFinder.tmpDistances)
				{
					if ((!cell.IsValid || keyValuePair.Value < num2) && keyValuePair.Key.Walkable(forPawn.Map) && !PawnUtility.AnyPawnBlockingPathAt(keyValuePair.Key, forPawn, false, false, false))
					{
						Building_Door door = keyValuePair.Key.GetDoor(forPawn.Map);
						if (door == null || door.FreePassage)
						{
							cell = keyValuePair.Key;
							num2 = keyValuePair.Value;
						}
					}
				}
				if (cell.IsValid)
				{
					goto Block_3;
				}
				if (radius > (float)forPawn.Map.Size.x && radius > (float)forPawn.Map.Size.z)
				{
					goto Block_10;
				}
				radius *= 2f;
				num = CellFinder.tmpDistances.Count;
			}
			return false;
			Block_3:
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
					Log.Error("Too many iterations.", false);
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
			Block_10:
			return false;
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x002F3ECC File Offset: 0x002F22CC
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
			CellFinder.tmpCells.Shuffle<IntVec3>();
			int j = 0;
			int count = CellFinder.tmpCells.Count;
			while (j < count)
			{
				if (predicate(CellFinder.tmpCells[j]))
				{
					result = CellFinder.tmpCells[j];
					return true;
				}
				j++;
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x002F3FE4 File Offset: 0x002F23E4
		public static IntVec3 RandomSpawnCellForPawnNear(IntVec3 root, Map map, int firstTryWithRadius = 4)
		{
			IntVec3 intVec;
			IntVec3 result;
			if (CellFinder.TryFindRandomSpawnCellForPawnNear(root, map, out intVec, firstTryWithRadius))
			{
				result = intVec;
			}
			else
			{
				result = root;
			}
			return result;
		}

		// Token: 0x06005D4B RID: 23883 RVA: 0x002F4010 File Offset: 0x002F2410
		public static bool TryFindRandomSpawnCellForPawnNear(IntVec3 root, Map map, out IntVec3 result, int firstTryWithRadius = 4)
		{
			bool result2;
			if (root.Standable(map) && root.GetFirstPawn(map) == null)
			{
				result = root;
				result2 = true;
			}
			else
			{
				bool rootFogged = root.Fogged(map);
				int num = firstTryWithRadius;
				for (int i = 0; i < 3; i++)
				{
					if (CellFinder.TryFindRandomReachableCellNear(root, map, (float)num, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 c) => c.Standable(map) && (rootFogged || !c.Fogged(map)) && c.GetFirstPawn(map) == null, null, out result, 999999))
					{
						return true;
					}
					num *= 2;
				}
				num = firstTryWithRadius + 1;
				while (!CellFinder.TryRandomClosewalkCellNear(root, map, num, out result, null))
				{
					if (num > map.Size.x / 2 && num > map.Size.z / 2)
					{
						result = root;
						return false;
					}
					num *= 2;
				}
				result2 = true;
			}
			return result2;
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x002F4134 File Offset: 0x002F2534
		public static IntVec3 FindNoWipeSpawnLocNear(IntVec3 near, Map map, ThingDef thingToSpawn, Rot4 rot, int maxDist = 2, Predicate<IntVec3> extraValidator = null)
		{
			int num = GenRadial.NumCellsInRadius((float)maxDist);
			IntVec3 intVec = IntVec3.Invalid;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec2 = near + GenRadial.RadialPattern[i];
				if (intVec2.InBounds(map))
				{
					CellRect cellRect = GenAdj.OccupiedRect(intVec2, rot, thingToSpawn.size);
					if (cellRect.InBounds(map))
					{
						if (GenSight.LineOfSight(near, intVec2, map, true, null, 0, 0))
						{
							if (extraValidator == null || extraValidator(intVec2))
							{
								if (thingToSpawn.category != ThingCategory.Building || GenConstruct.CanBuildOnTerrain(thingToSpawn, intVec2, map, rot, null))
								{
									bool flag = false;
									bool flag2 = false;
									CellFinder.tmpUniqueWipedThings.Clear();
									CellRect.CellRectIterator iterator = cellRect.GetIterator();
									while (!iterator.Done())
									{
										if (iterator.Current.Impassable(map))
										{
											flag2 = true;
										}
										List<Thing> thingList = iterator.Current.GetThingList(map);
										for (int j = 0; j < thingList.Count; j++)
										{
											if (thingList[j] is Pawn)
											{
												flag = true;
											}
											else if (GenSpawn.SpawningWipes(thingToSpawn, thingList[j].def) && !CellFinder.tmpUniqueWipedThings.Contains(thingList[j]))
											{
												CellFinder.tmpUniqueWipedThings.Add(thingList[j]);
											}
										}
										iterator.MoveNext();
									}
									if (flag && thingToSpawn.passability == Traversability.Impassable)
									{
										CellFinder.tmpUniqueWipedThings.Clear();
									}
									else if (flag2 && thingToSpawn.category == ThingCategory.Item)
									{
										CellFinder.tmpUniqueWipedThings.Clear();
									}
									else
									{
										float num3 = 0f;
										for (int k = 0; k < CellFinder.tmpUniqueWipedThings.Count; k++)
										{
											if (CellFinder.tmpUniqueWipedThings[k].def.category == ThingCategory.Building && !CellFinder.tmpUniqueWipedThings[k].def.costList.NullOrEmpty<ThingDefCountClass>() && CellFinder.tmpUniqueWipedThings[k].def.costStuffCount == 0)
											{
												List<ThingDefCountClass> list = CellFinder.tmpUniqueWipedThings[k].CostListAdjusted();
												for (int l = 0; l < list.Count; l++)
												{
													num3 += list[l].thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)list[l].count * (float)CellFinder.tmpUniqueWipedThings[k].stackCount;
												}
											}
											else
											{
												num3 += CellFinder.tmpUniqueWipedThings[k].MarketValue * (float)CellFinder.tmpUniqueWipedThings[k].stackCount;
											}
											if (CellFinder.tmpUniqueWipedThings[k].def.category == ThingCategory.Building || CellFinder.tmpUniqueWipedThings[k].def.category == ThingCategory.Item)
											{
												num3 = Mathf.Max(num3, 0.001f);
											}
										}
										CellFinder.tmpUniqueWipedThings.Clear();
										if (!intVec.IsValid || num3 < num2)
										{
											if (num3 == 0f)
											{
												return intVec2;
											}
											intVec = intVec2;
											num2 = num3;
										}
									}
								}
							}
						}
					}
				}
			}
			return (!intVec.IsValid) ? near : intVec;
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x002F44E4 File Offset: 0x002F28E4
		private static IEnumerable<IntVec3> GetAdjacentCardinalCellsForBestStandCell(IntVec3 x, float radius, Pawn pawn)
		{
			if ((float)(x - pawn.Position).LengthManhattan > radius)
			{
				yield break;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = x + GenAdj.CardinalDirections[i];
				if (c.InBounds(pawn.Map) && c.Walkable(pawn.Map))
				{
					Building_Door door = c.GetEdifice(pawn.Map) as Building_Door;
					if (door == null || door.CanPhysicallyPass(pawn))
					{
						yield return c;
					}
				}
			}
			yield break;
		}

		// Token: 0x04003DA6 RID: 15782
		public static float EdgeRoadChance_Ignore = 0f;

		// Token: 0x04003DA7 RID: 15783
		public static float EdgeRoadChance_Animal = 0f;

		// Token: 0x04003DA8 RID: 15784
		public static float EdgeRoadChance_Hostile = 0.2f;

		// Token: 0x04003DA9 RID: 15785
		public static float EdgeRoadChance_Neutral = 0.75f;

		// Token: 0x04003DAA RID: 15786
		public static float EdgeRoadChance_Friendly = 0.75f;

		// Token: 0x04003DAB RID: 15787
		public static float EdgeRoadChance_Always = 1f;

		// Token: 0x04003DAC RID: 15788
		private static List<IntVec3> workingCells = new List<IntVec3>();

		// Token: 0x04003DAD RID: 15789
		private static List<Region> workingRegions = new List<Region>();

		// Token: 0x04003DAE RID: 15790
		private static List<int> workingListX = new List<int>();

		// Token: 0x04003DAF RID: 15791
		private static List<int> workingListZ = new List<int>();

		// Token: 0x04003DB0 RID: 15792
		private static List<IntVec3> mapEdgeCells;

		// Token: 0x04003DB1 RID: 15793
		private static IntVec3 mapEdgeCellsSize;

		// Token: 0x04003DB2 RID: 15794
		private static List<IntVec3>[] mapSingleEdgeCells = new List<IntVec3>[4];

		// Token: 0x04003DB3 RID: 15795
		private static IntVec3 mapSingleEdgeCellsSize;

		// Token: 0x04003DB4 RID: 15796
		private static Dictionary<IntVec3, float> tmpDistances = new Dictionary<IntVec3, float>();

		// Token: 0x04003DB5 RID: 15797
		private static Dictionary<IntVec3, IntVec3> tmpParents = new Dictionary<IntVec3, IntVec3>();

		// Token: 0x04003DB6 RID: 15798
		private static List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x04003DB7 RID: 15799
		private static List<Thing> tmpUniqueWipedThings = new List<Thing>();
	}
}
