using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056C RID: 1388
	public class FeatureWorker_Island : FeatureWorker_FloodFill
	{
		// Token: 0x06001A4F RID: 6735 RVA: 0x000E4150 File Offset: 0x000E2550
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x000E4190 File Offset: 0x000E2590
		protected override bool IsPossiblyAllowed(int tile)
		{
			return Find.WorldGrid[tile].biome == BiomeDefOf.Lake;
		}
	}
}
