using System;

namespace Verse
{
	// Token: 0x02000ED0 RID: 3792
	public class CellIndices
	{
		// Token: 0x060059EE RID: 23022 RVA: 0x002E31EC File Offset: 0x002E15EC
		public CellIndices(Map map)
		{
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x060059EF RID: 23023 RVA: 0x002E3228 File Offset: 0x002E1628
		public int NumGridCells
		{
			get
			{
				return this.mapSizeX * this.mapSizeZ;
			}
		}

		// Token: 0x060059F0 RID: 23024 RVA: 0x002E324C File Offset: 0x002E164C
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.mapSizeX);
		}

		// Token: 0x060059F1 RID: 23025 RVA: 0x002E3270 File Offset: 0x002E1670
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
		}

		// Token: 0x060059F2 RID: 23026 RVA: 0x002E3294 File Offset: 0x002E1694
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.mapSizeX);
		}

		// Token: 0x04003C12 RID: 15378
		private int mapSizeX;

		// Token: 0x04003C13 RID: 15379
		private int mapSizeZ;
	}
}
