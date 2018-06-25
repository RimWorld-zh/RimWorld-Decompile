using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056E RID: 1390
	public class FeatureWorker_Island : FeatureWorker_FloodFill
	{
		// Token: 0x06001A53 RID: 6739 RVA: 0x000E42A0 File Offset: 0x000E26A0
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x000E42E0 File Offset: 0x000E26E0
		protected override bool IsPossiblyAllowed(int tile)
		{
			return Find.WorldGrid[tile].biome == BiomeDefOf.Lake;
		}
	}
}
