using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000570 RID: 1392
	public class FeatureWorker_Peninsula : FeatureWorker_Protrusion
	{
		// Token: 0x06001A58 RID: 6744 RVA: 0x000E494C File Offset: 0x000E2D4C
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}
	}
}
