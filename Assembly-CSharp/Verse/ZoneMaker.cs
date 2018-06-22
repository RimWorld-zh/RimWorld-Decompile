using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CB9 RID: 3257
	public static class ZoneMaker
	{
		// Token: 0x060047E1 RID: 18401 RVA: 0x0025D778 File Offset: 0x0025BB78
		public static Zone MakeZoneWithCells(Zone z, IEnumerable<IntVec3> cells)
		{
			if (cells != null)
			{
				foreach (IntVec3 c in cells)
				{
					z.AddCell(c);
				}
			}
			return z;
		}
	}
}
