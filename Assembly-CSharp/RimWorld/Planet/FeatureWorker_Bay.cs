using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000571 RID: 1393
	public class FeatureWorker_Bay : FeatureWorker_Protrusion
	{
		// Token: 0x06001A5B RID: 6747 RVA: 0x000E4764 File Offset: 0x000E2B64
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}
	}
}
