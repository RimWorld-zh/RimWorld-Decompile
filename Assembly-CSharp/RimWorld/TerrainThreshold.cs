using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000286 RID: 646
	public class TerrainThreshold
	{
		// Token: 0x0400055D RID: 1373
		public TerrainDef terrain;

		// Token: 0x0400055E RID: 1374
		public float min = -1000f;

		// Token: 0x0400055F RID: 1375
		public float max = 1000f;

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00063A70 File Offset: 0x00061E70
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
