using System;

namespace Verse
{
	// Token: 0x02000ED3 RID: 3795
	public class CellIndices
	{
		// Token: 0x04003C1A RID: 15386
		private int mapSizeX;

		// Token: 0x04003C1B RID: 15387
		private int mapSizeZ;

		// Token: 0x060059F1 RID: 23025 RVA: 0x002E352C File Offset: 0x002E192C
		public CellIndices(Map map)
		{
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x060059F2 RID: 23026 RVA: 0x002E3568 File Offset: 0x002E1968
		public int NumGridCells
		{
			get
			{
				return this.mapSizeX * this.mapSizeZ;
			}
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x002E358C File Offset: 0x002E198C
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.mapSizeX);
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x002E35B0 File Offset: 0x002E19B0
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x002E35D4 File Offset: 0x002E19D4
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.mapSizeX);
		}
	}
}
