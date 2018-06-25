using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000288 RID: 648
	public class TerrainThreshold
	{
		// Token: 0x0400055F RID: 1375
		public TerrainDef terrain;

		// Token: 0x04000560 RID: 1376
		public float min = -1000f;

		// Token: 0x04000561 RID: 1377
		public float max = 1000f;

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00063BBC File Offset: 0x00061FBC
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
