using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000570 RID: 1392
	public class FeatureWorker_Island : FeatureWorker_FloodFill
	{
		// Token: 0x06001A58 RID: 6744 RVA: 0x000E40FC File Offset: 0x000E24FC
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x000E413C File Offset: 0x000E253C
		protected override bool IsPossiblyAllowed(int tile)
		{
			return Find.WorldGrid[tile].biome == BiomeDefOf.Lake;
		}
	}
}
