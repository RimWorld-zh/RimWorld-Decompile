using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056C RID: 1388
	public class FeatureWorker_Biome : FeatureWorker_FloodFill
	{
		// Token: 0x06001A42 RID: 6722 RVA: 0x000E4490 File Offset: 0x000E2890
		protected override bool IsRoot(int tile)
		{
			return this.def.rootBiomes.Contains(Find.WorldGrid[tile].biome);
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x000E44C8 File Offset: 0x000E28C8
		protected override bool IsPossiblyAllowed(int tile)
		{
			return this.def.acceptableBiomes.Contains(Find.WorldGrid[tile].biome);
		}
	}
}
