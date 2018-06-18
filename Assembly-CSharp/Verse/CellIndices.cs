using System;

namespace Verse
{
	// Token: 0x02000ED1 RID: 3793
	public class CellIndices
	{
		// Token: 0x060059CD RID: 22989 RVA: 0x002E13D8 File Offset: 0x002DF7D8
		public CellIndices(Map map)
		{
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x060059CE RID: 22990 RVA: 0x002E1414 File Offset: 0x002DF814
		public int NumGridCells
		{
			get
			{
				return this.mapSizeX * this.mapSizeZ;
			}
		}

		// Token: 0x060059CF RID: 22991 RVA: 0x002E1438 File Offset: 0x002DF838
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.mapSizeX);
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x002E145C File Offset: 0x002DF85C
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x002E1480 File Offset: 0x002DF880
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.mapSizeX);
		}

		// Token: 0x04003C02 RID: 15362
		private int mapSizeX;

		// Token: 0x04003C03 RID: 15363
		private int mapSizeZ;
	}
}
