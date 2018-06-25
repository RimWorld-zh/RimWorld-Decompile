using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056F RID: 1391
	public class FeatureWorker_Bay : FeatureWorker_Protrusion
	{
		// Token: 0x06001A56 RID: 6742 RVA: 0x000E4908 File Offset: 0x000E2D08
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}
	}
}
