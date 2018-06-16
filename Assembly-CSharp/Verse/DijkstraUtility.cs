using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F1C RID: 3868
	public static class DijkstraUtility
	{
		// Token: 0x06005CA2 RID: 23714 RVA: 0x002EF11C File Offset: 0x002ED51C
		public static IEnumerable<IntVec3> AdjacentCellsNeighborsGetter(IntVec3 cell, Map map)
		{
			DijkstraUtility.adjacentCells.Clear();
			IntVec3[] array = GenAdj.AdjacentCells;
			for (int i = 0; i < array.Length; i++)
			{
				IntVec3 intVec = cell + array[i];
				if (intVec.InBounds(map))
				{
					DijkstraUtility.adjacentCells.Add(intVec);
				}
			}
			return DijkstraUtility.adjacentCells;
		}

		// Token: 0x04003D82 RID: 15746
		private static List<IntVec3> adjacentCells = new List<IntVec3>();
	}
}
