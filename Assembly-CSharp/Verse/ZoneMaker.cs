using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CBC RID: 3260
	public static class ZoneMaker
	{
		// Token: 0x060047D8 RID: 18392 RVA: 0x0025C388 File Offset: 0x0025A788
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
