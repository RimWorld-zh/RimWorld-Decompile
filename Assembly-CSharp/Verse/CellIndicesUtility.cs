using System;

namespace Verse
{
	// Token: 0x02000ED3 RID: 3795
	public static class CellIndicesUtility
	{
		// Token: 0x060059F6 RID: 23030 RVA: 0x002E33D8 File Offset: 0x002E17D8
		public static int CellToIndex(IntVec3 c, int mapSizeX)
		{
			return c.z * mapSizeX + c.x;
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x002E3400 File Offset: 0x002E1800
		public static int CellToIndex(int x, int z, int mapSizeX)
		{
			return z * mapSizeX + x;
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x002E341C File Offset: 0x002E181C
		public static IntVec3 IndexToCell(int ind, int mapSizeX)
		{
			int newX = ind % mapSizeX;
			int newZ = ind / mapSizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}
