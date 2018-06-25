using System;

namespace Verse
{
	// Token: 0x02000ED2 RID: 3794
	public class CellIndices
	{
		// Token: 0x04003C12 RID: 15378
		private int mapSizeX;

		// Token: 0x04003C13 RID: 15379
		private int mapSizeZ;

		// Token: 0x060059F1 RID: 23025 RVA: 0x002E330C File Offset: 0x002E170C
		public CellIndices(Map map)
		{
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x060059F2 RID: 23026 RVA: 0x002E3348 File Offset: 0x002E1748
		public int NumGridCells
		{
			get
			{
				return this.mapSizeX * this.mapSizeZ;
			}
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x002E336C File Offset: 0x002E176C
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.mapSizeX);
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x002E3390 File Offset: 0x002E1790
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x002E33B4 File Offset: 0x002E17B4
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.mapSizeX);
		}
	}
}
