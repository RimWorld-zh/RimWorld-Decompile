using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056C RID: 1388
	public class FeatureWorker_Biome : FeatureWorker_FloodFill
	{
		// Token: 0x06001A43 RID: 6723 RVA: 0x000E4228 File Offset: 0x000E2628
		protected override bool IsRoot(int tile)
		{
			return this.def.rootBiomes.Contains(Find.WorldGrid[tile].biome);
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x000E4260 File Offset: 0x000E2660
		protected override bool IsPossiblyAllowed(int tile)
		{
			return this.def.acceptableBiomes.Contains(Find.WorldGrid[tile].biome);
		}
	}
}
