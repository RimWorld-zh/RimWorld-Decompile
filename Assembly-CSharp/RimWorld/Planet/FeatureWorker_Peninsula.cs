using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000570 RID: 1392
	public class FeatureWorker_Peninsula : FeatureWorker_Protrusion
	{
		// Token: 0x06001A57 RID: 6743 RVA: 0x000E4BB4 File Offset: 0x000E2FB4
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}
	}
}
