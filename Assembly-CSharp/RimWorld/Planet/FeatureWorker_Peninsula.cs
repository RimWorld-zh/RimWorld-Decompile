using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000572 RID: 1394
	public class FeatureWorker_Peninsula : FeatureWorker_Protrusion
	{
		// Token: 0x06001A5D RID: 6749 RVA: 0x000E47A8 File Offset: 0x000E2BA8
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}
	}
}
