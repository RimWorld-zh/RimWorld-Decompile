using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CBD RID: 3261
	public static class ZoneMaker
	{
		// Token: 0x060047DA RID: 18394 RVA: 0x0025C3B0 File Offset: 0x0025A7B0
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
