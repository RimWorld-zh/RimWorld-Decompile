using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000570 RID: 1392
	public class FeatureWorker_Island : FeatureWorker_FloodFill
	{
		// Token: 0x06001A57 RID: 6743 RVA: 0x000E40A8 File Offset: 0x000E24A8
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x000E40E8 File Offset: 0x000E24E8
		protected override bool IsPossiblyAllowed(int tile)
		{
			return Find.WorldGrid[tile].biome == BiomeDefOf.Lake;
		}
	}
}
