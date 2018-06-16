using System;

namespace Verse
{
	// Token: 0x02000ED2 RID: 3794
	public class CellIndices
	{
		// Token: 0x060059CF RID: 22991 RVA: 0x002E1300 File Offset: 0x002DF700
		public CellIndices(Map map)
		{
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x060059D0 RID: 22992 RVA: 0x002E133C File Offset: 0x002DF73C
		public int NumGridCells
		{
			get
			{
				return this.mapSizeX * this.mapSizeZ;
			}
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x002E1360 File Offset: 0x002DF760
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.mapSizeX);
		}

		// Token: 0x060059D2 RID: 22994 RVA: 0x002E1384 File Offset: 0x002DF784
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x002E13A8 File Offset: 0x002DF7A8
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.mapSizeX);
		}

		// Token: 0x04003C03 RID: 15363
		private int mapSizeX;

		// Token: 0x04003C04 RID: 15364
		private int mapSizeZ;
	}
}
