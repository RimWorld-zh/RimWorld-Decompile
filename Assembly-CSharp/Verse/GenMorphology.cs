using System;
using System.Collections.Generic;

namespace Verse
{
	public static class GenMorphology
	{
		private static HashSet<IntVec3> tmpOutput = new HashSet<IntVec3>();

		private static HashSet<IntVec3> cellsSet = new HashSet<IntVec3>();

		private static List<IntVec3> tmpEdgeCells = new List<IntVec3>();

		public static void Erode(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count > 0)
			{
				IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
				GenMorphology.cellsSet.Clear();
				GenMorphology.cellsSet.AddRange(cells);
				GenMorphology.tmpEdgeCells.Clear();
				for (int i = 0; i < cells.Count; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						IntVec3 item = cells[i] + cardinalDirections[j];
						if (!GenMorphology.cellsSet.Contains(item))
						{
							GenMorphology.tmpEdgeCells.Add(cells[i]);
							break;
						}
					}
				}
				if (GenMorphology.tmpEdgeCells.Any())
				{
					GenMorphology.tmpOutput.Clear();
					Predicate<IntVec3> predicate = (extraPredicate == null) ? ((Predicate<IntVec3>)((IntVec3 x) => GenMorphology.cellsSet.Contains(x))) : ((Predicate<IntVec3>)((IntVec3 x) => GenMorphology.cellsSet.Contains(x) && extraPredicate(x)));
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 invalid = IntVec3.Invalid;
					Predicate<IntVec3> passCheck = predicate;
					Func<IntVec3, int, bool> processor = delegate(IntVec3 cell, int traversalDist)
					{
						if (traversalDist >= count)
						{
							GenMorphology.tmpOutput.Add(cell);
						}
						return false;
					};
					List<IntVec3> extraRoots = GenMorphology.tmpEdgeCells;
					floodFiller.FloodFill(invalid, passCheck, processor, 2147483647, false, extraRoots);
					cells.Clear();
					cells.AddRange(GenMorphology.tmpOutput);
				}
			}
		}

		public static void Dilate(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count > 0)
			{
				FloodFiller floodFiller = map.floodFiller;
				IntVec3 invalid = IntVec3.Invalid;
				Predicate<IntVec3> passCheck = extraPredicate ?? ((Predicate<IntVec3>)((IntVec3 x) => true));
				Func<IntVec3, int, bool> processor = delegate(IntVec3 cell, int traversalDist)
				{
					if (traversalDist > count)
					{
						return true;
					}
					if (traversalDist != 0)
					{
						cells.Add(cell);
					}
					return false;
				};
				List<IntVec3> extraRoots = cells;
				floodFiller.FloodFill(invalid, passCheck, processor, 2147483647, false, extraRoots);
			}
		}

		public static void Open(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Erode(cells, count, map, null);
			GenMorphology.Dilate(cells, count, map, null);
		}

		public static void Close(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Dilate(cells, count, map, null);
			GenMorphology.Erode(cells, count, map, null);
		}
	}
}
