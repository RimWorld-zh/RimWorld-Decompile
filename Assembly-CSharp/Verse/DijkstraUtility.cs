using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F1B RID: 3867
	public static class DijkstraUtility
	{
		// Token: 0x06005CC8 RID: 23752 RVA: 0x002F1224 File Offset: 0x002EF624
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

		// Token: 0x04003D93 RID: 15763
		private static List<IntVec3> adjacentCells = new List<IntVec3>();
	}
}
