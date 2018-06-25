using System;

namespace Verse
{
	// Token: 0x02000ED4 RID: 3796
	public static class CellIndicesUtility
	{
		// Token: 0x060059F6 RID: 23030 RVA: 0x002E35F8 File Offset: 0x002E19F8
		public static int CellToIndex(IntVec3 c, int mapSizeX)
		{
			return c.z * mapSizeX + c.x;
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x002E3620 File Offset: 0x002E1A20
		public static int CellToIndex(int x, int z, int mapSizeX)
		{
			return z * mapSizeX + x;
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x002E363C File Offset: 0x002E1A3C
		public static IntVec3 IndexToCell(int ind, int mapSizeX)
		{
			int newX = ind % mapSizeX;
			int newZ = ind / mapSizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}
