using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056A RID: 1386
	public class FeatureWorker_Biome : FeatureWorker_FloodFill
	{
		// Token: 0x06001A3F RID: 6719 RVA: 0x000E40D8 File Offset: 0x000E24D8
		protected override bool IsRoot(int tile)
		{
			return this.def.rootBiomes.Contains(Find.WorldGrid[tile].biome);
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x000E4110 File Offset: 0x000E2510
		protected override bool IsPossiblyAllowed(int tile)
		{
			return this.def.acceptableBiomes.Contains(Find.WorldGrid[tile].biome);
		}
	}
}
