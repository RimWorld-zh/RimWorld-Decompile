using System;

namespace Verse
{
	// Token: 0x02000ED1 RID: 3793
	public static class CellIndicesUtility
	{
		// Token: 0x060059F3 RID: 23027 RVA: 0x002E32B8 File Offset: 0x002E16B8
		public static int CellToIndex(IntVec3 c, int mapSizeX)
		{
			return c.z * mapSizeX + c.x;
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x002E32E0 File Offset: 0x002E16E0
		public static int CellToIndex(int x, int z, int mapSizeX)
		{
			return z * mapSizeX + x;
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x002E32FC File Offset: 0x002E16FC
		public static IntVec3 IndexToCell(int ind, int mapSizeX)
		{
			int newX = ind % mapSizeX;
			int newZ = ind / mapSizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}
