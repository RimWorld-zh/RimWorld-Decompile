using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000288 RID: 648
	public class TerrainThreshold
	{
		// Token: 0x0400055D RID: 1373
		public TerrainDef terrain;

		// Token: 0x0400055E RID: 1374
		public float min = -1000f;

		// Token: 0x0400055F RID: 1375
		public float max = 1000f;

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00063BC0 File Offset: 0x00061FC0
		public static TerrainDef TerrainAtValue(List<TerrainThreshold> threshes, float val)
		{
			for (int i = 0; i < threshes.Count; i++)
			{
				if (threshes[i].min <= val && threshes[i].max >= val)
				{
					return threshes[i].terrain;
				}
			}
			return null;
		}
	}
}
