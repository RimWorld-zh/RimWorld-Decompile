using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F46 RID: 3910
	public static class GenMorphology
	{
		// Token: 0x06005E6F RID: 24175 RVA: 0x002FF114 File Offset: 0x002FD514
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
				if (GenMorphology.tmpEdgeCells.Any<IntVec3>())
				{
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
			}
		}

		// Token: 0x06005E70 RID: 24176 RVA: 0x002FF280 File Offset: 0x002FD680
		public static void Dilate(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count > 0)
			{
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
					bool result;
					if (traversalDist > count)
					{
						result = true;
					}
					else
					{
						if (traversalDist != 0)
						{
							cells.Add(cell);
						}
						result = false;
					}
					return result;
				};
				List<IntVec3> cells2 = cells;
				floodFiller.FloodFill(invalid, passCheck, processor, int.MaxValue, false, cells2);
			}
		}

		// Token: 0x06005E71 RID: 24177 RVA: 0x002FF30A File Offset: 0x002FD70A
		public static void Open(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Erode(cells, count, map, null);
			GenMorphology.Dilate(cells, count, map, null);
		}

		// Token: 0x06005E72 RID: 24178 RVA: 0x002FF31F File Offset: 0x002FD71F
		public static void Close(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Dilate(cells, count, map, null);
			GenMorphology.Erode(cells, count, map, null);
		}

		// Token: 0x04003E10 RID: 15888
		private static HashSet<IntVec3> tmpOutput = new HashSet<IntVec3>();

		// Token: 0x04003E11 RID: 15889
		private static HashSet<IntVec3> cellsSet = new HashSet<IntVec3>();

		// Token: 0x04003E12 RID: 15890
		private static List<IntVec3> tmpEdgeCells = new List<IntVec3>();
	}
}
