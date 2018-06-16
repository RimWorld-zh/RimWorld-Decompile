using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056E RID: 1390
	public class FeatureWorker_Biome : FeatureWorker_FloodFill
	{
		// Token: 0x06001A47 RID: 6727 RVA: 0x000E4030 File Offset: 0x000E2430
		protected override bool IsRoot(int tile)
		{
			return this.def.rootBiomes.Contains(Find.WorldGrid[tile].biome);
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x000E4068 File Offset: 0x000E2468
		protected override bool IsPossiblyAllowed(int tile)
		{
			return this.def.acceptableBiomes.Contains(Find.WorldGrid[tile].biome);
		}
	}
}
