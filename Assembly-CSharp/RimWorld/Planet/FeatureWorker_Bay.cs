using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056D RID: 1389
	public class FeatureWorker_Bay : FeatureWorker_Protrusion
	{
		// Token: 0x06001A52 RID: 6738 RVA: 0x000E47B8 File Offset: 0x000E2BB8
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}
	}
}
