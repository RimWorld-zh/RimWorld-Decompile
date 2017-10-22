using System;
using System.Collections.Generic;

namespace Verse
{
	public static class RoofCollapseCellsFinder
	{
		private static List<IntVec3> roofsCollapsingBecauseTooFar = new List<IntVec3>();

		private static HashSet<IntVec3> visitedCells = new HashSet<IntVec3>();

		public static void Notify_RoofHolderDespawned(Thing t, Map map)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				RoofCollapseCellsFinder.ProcessRoofHolderDespawned(t.OccupiedRect(), t.Position, map, false);
			}
		}

		public static void ProcessRoofHolderDespawned(CellRect rect, IntVec3 position, Map map, bool removalMode = false)
		{
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(rect, map);
			RoofGrid roofGrid = map.roofGrid;
			RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Clear();
			for (int i = 0; i < RoofCollapseUtility.RoofSupportRadialCellsCount; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && roofGrid.Roofed(intVec.x, intVec.z) && !map.roofCollapseBuffer.IsMarkedToCollapse(intVec) && !RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, map))
				{
					if (removalMode)
					{
						map.roofGrid.SetRoof(intVec, null);
					}
					else
					{
						map.roofCollapseBuffer.MarkToCollapse(intVec);
					}
					RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Add(intVec);
				}
			}
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar, map, removalMode);
			RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Clear();
		}

		public static void RemoveBulkCollapsingRoofs(List<IntVec3> nearCells, Map map)
		{
			for (int i = 0; i < nearCells.Count; i++)
			{
				IntVec3 intVec = nearCells[i];
				int x = intVec.x;
				IntVec3 intVec2 = nearCells[i];
				RoofCollapseCellsFinder.ProcessRoofHolderDespawned(new CellRect(x, intVec2.z, 1, 1), nearCells[i], map, true);
			}
		}

		public static void CheckCollapseFlyingRoofs(List<IntVec3> nearCells, Map map, bool removalMode = false)
		{
			RoofCollapseCellsFinder.visitedCells.Clear();
			for (int i = 0; i < nearCells.Count; i++)
			{
				RoofCollapseCellsFinder.CheckCollapseFlyingRoofAtAndAdjInternal(nearCells[i], map, removalMode);
			}
			RoofCollapseCellsFinder.visitedCells.Clear();
		}

		public static void CheckCollapseFlyingRoofs(CellRect nearRect, Map map)
		{
			RoofCollapseCellsFinder.visitedCells.Clear();
			CellRect.CellRectIterator iterator = nearRect.GetIterator();
			while (!iterator.Done())
			{
				RoofCollapseCellsFinder.CheckCollapseFlyingRoofAtAndAdjInternal(iterator.Current, map, false);
				iterator.MoveNext();
			}
			RoofCollapseCellsFinder.visitedCells.Clear();
		}

		private static bool CheckCollapseFlyingRoofAtAndAdjInternal(IntVec3 root, Map map, bool removalMode)
		{
			ProfilerThreadCheck.BeginSample("CheckCollapseFlyingRoofAtInternal()");
			RoofCollapseBuffer roofCollapseBuffer = map.roofCollapseBuffer;
			if (removalMode && roofCollapseBuffer.CellsMarkedToCollapse.Count > 0)
			{
				map.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			}
			for (int i = 0; i < 5; i++)
			{
				IntVec3 intVec = root + GenAdj.CardinalDirectionsAndInside[i];
				if (intVec.InBounds(map) && intVec.Roofed(map) && !RoofCollapseCellsFinder.visitedCells.Contains(intVec) && !roofCollapseBuffer.IsMarkedToCollapse(intVec) && !RoofCollapseCellsFinder.ConnectsToRoofHolder(intVec, map))
				{
					map.floodFiller.FloodFill(intVec, (Predicate<IntVec3>)((IntVec3 x) => x.Roofed(map)), (Action<IntVec3>)delegate(IntVec3 x)
					{
						roofCollapseBuffer.MarkToCollapse(x);
					}, false);
					if (removalMode)
					{
						for (int j = 0; j < roofCollapseBuffer.CellsMarkedToCollapse.Count; j++)
						{
							map.roofGrid.SetRoof(roofCollapseBuffer.CellsMarkedToCollapse[j], null);
						}
						roofCollapseBuffer.Clear();
					}
				}
			}
			ProfilerThreadCheck.EndSample();
			return false;
		}

		private static bool ConnectsToRoofHolder(IntVec3 c, Map map)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (Predicate<IntVec3>)((IntVec3 x) => x.Roofed(map) && !connected), (Action<IntVec3>)delegate(IntVec3 x)
			{
				if (RoofCollapseCellsFinder.visitedCells.Contains(x))
				{
					connected = true;
				}
				else
				{
					RoofCollapseCellsFinder.visitedCells.Add(x);
					int num = 0;
					while (true)
					{
						if (num < 5)
						{
							IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[num];
							if (c2.InBounds(map))
							{
								Building edifice = c2.GetEdifice(map);
								if (edifice != null && edifice.def.holdsRoof)
									break;
							}
							num++;
							continue;
						}
						return;
					}
					connected = true;
				}
			}, false);
			return connected;
		}
	}
}
