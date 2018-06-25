using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F1F RID: 3871
	public static class DijkstraUtility
	{
		// Token: 0x04003D96 RID: 15766
		private static List<IntVec3> adjacentCells = new List<IntVec3>();

		// Token: 0x06005CD2 RID: 23762 RVA: 0x002F18A4 File Offset: 0x002EFCA4
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
	}
}
