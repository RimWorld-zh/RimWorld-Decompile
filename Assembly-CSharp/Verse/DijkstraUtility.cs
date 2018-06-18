using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F1B RID: 3867
	public static class DijkstraUtility
	{
		// Token: 0x06005CA0 RID: 23712 RVA: 0x002EF1F8 File Offset: 0x002ED5F8
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

		// Token: 0x04003D81 RID: 15745
		private static List<IntVec3> adjacentCells = new List<IntVec3>();
	}
}
