using System;

namespace Verse
{
	// Token: 0x02000ED3 RID: 3795
	public static class CellIndicesUtility
	{
		// Token: 0x060059D4 RID: 22996 RVA: 0x002E13CC File Offset: 0x002DF7CC
		public static int CellToIndex(IntVec3 c, int mapSizeX)
		{
			return c.z * mapSizeX + c.x;
		}

		// Token: 0x060059D5 RID: 22997 RVA: 0x002E13F4 File Offset: 0x002DF7F4
		public static int CellToIndex(int x, int z, int mapSizeX)
		{
			return z * mapSizeX + x;
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x002E1410 File Offset: 0x002DF810
		public static IntVec3 IndexToCell(int ind, int mapSizeX)
		{
			int newX = ind % mapSizeX;
			int newZ = ind / mapSizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}
