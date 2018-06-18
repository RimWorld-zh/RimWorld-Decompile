using System;

namespace Verse
{
	// Token: 0x02000ED2 RID: 3794
	public static class CellIndicesUtility
	{
		// Token: 0x060059D2 RID: 22994 RVA: 0x002E14A4 File Offset: 0x002DF8A4
		public static int CellToIndex(IntVec3 c, int mapSizeX)
		{
			return c.z * mapSizeX + c.x;
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x002E14CC File Offset: 0x002DF8CC
		public static int CellToIndex(int x, int z, int mapSizeX)
		{
			return z * mapSizeX + x;
		}

		// Token: 0x060059D4 RID: 22996 RVA: 0x002E14E8 File Offset: 0x002DF8E8
		public static IntVec3 IndexToCell(int ind, int mapSizeX)
		{
			int newX = ind % mapSizeX;
			int newZ = ind / mapSizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}
