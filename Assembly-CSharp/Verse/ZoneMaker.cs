using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CBB RID: 3259
	public static class ZoneMaker
	{
		// Token: 0x060047E4 RID: 18404 RVA: 0x0025D854 File Offset: 0x0025BC54
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
