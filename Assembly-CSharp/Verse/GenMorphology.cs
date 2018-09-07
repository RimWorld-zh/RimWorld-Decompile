using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class GenMorphology
	{
		private static HashSet<IntVec3> tmpOutput = new HashSet<IntVec3>();

		private static HashSet<IntVec3> cellsSet = new HashSet<IntVec3>();

		private static List<IntVec3> tmpEdgeCells = new List<IntVec3>();

		[CompilerGenerated]
		private static Predicate<IntVec3> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<IntVec3> <>f__am$cache1;

		public static void Erode(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
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
			if (!GenMorphology.tmpEdgeCells.Any<IntVec3>())
			{
				return;
			}
			GenMorphology.tmpOutput.Clear();
			Predicate<IntVec3> predicate;
			if (extraPredicate != null)
			{
				predicate = ((IntVec3 x) => GenMorphology.cellsSet.Contains(x) && extraPredicate(x));
			}
			else
			{
				predicate = ((IntVec3 x) => GenMorphology.cellsSet.Contains(x));
			}
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
			floodFiller.FloodFill(invalid, passCheck, processor, int.MaxValue, false, extraRoots);
			cells.Clear();
			cells.AddRange(GenMorphology.tmpOutput);
		}

		public static void Dilate(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			FloodFiller floodFiller = map.floodFiller;
			IntVec3 invalid = IntVec3.Invalid;
			Predicate<IntVec3> predicate = extraPredicate;
			if (extraPredicate == null)
			{
				predicate = ((IntVec3 x) => true);
			}
			Predicate<IntVec3> passCheck = predicate;
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
			List<IntVec3> cells2 = cells;
			floodFiller.FloodFill(invalid, passCheck, processor, int.MaxValue, false, cells2);
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

		// Note: this type is marked as 'beforefieldinit'.
		static GenMorphology()
		{
		}

		[CompilerGenerated]
		private static bool <Erode>m__0(IntVec3 x)
		{
			return GenMorphology.cellsSet.Contains(x);
		}

		[CompilerGenerated]
		private static bool <Dilate>m__1(IntVec3 x)
		{
			return true;
		}

		[CompilerGenerated]
		private sealed class <Erode>c__AnonStorey0
		{
			internal Predicate<IntVec3> extraPredicate;

			internal int count;

			public <Erode>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return GenMorphology.cellsSet.Contains(x) && this.extraPredicate(x);
			}

			internal bool <>m__1(IntVec3 cell, int traversalDist)
			{
				if (traversalDist >= this.count)
				{
					GenMorphology.tmpOutput.Add(cell);
				}
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <Dilate>c__AnonStorey1
		{
			internal int count;

			internal List<IntVec3> cells;

			public <Dilate>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IntVec3 cell, int traversalDist)
			{
				if (traversalDist > this.count)
				{
					return true;
				}
				if (traversalDist != 0)
				{
					this.cells.Add(cell);
				}
				return false;
			}
		}
	}
}
