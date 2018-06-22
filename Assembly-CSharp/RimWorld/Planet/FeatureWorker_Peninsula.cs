using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056E RID: 1390
	public class FeatureWorker_Peninsula : FeatureWorker_Protrusion
	{
		// Token: 0x06001A54 RID: 6740 RVA: 0x000E47FC File Offset: 0x000E2BFC
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}
	}
}
