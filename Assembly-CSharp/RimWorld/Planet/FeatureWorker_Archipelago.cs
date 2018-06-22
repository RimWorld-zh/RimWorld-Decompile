using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000566 RID: 1382
	public class FeatureWorker_Archipelago : FeatureWorker_Cluster
	{
		// Token: 0x06001A25 RID: 6693 RVA: 0x000E3550 File Offset: 0x000E1950
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x000E3590 File Offset: 0x000E1990
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			return true;
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x000E35AC File Offset: 0x000E19AC
		protected override bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			bool flag;
			return base.IsMember(tile, out flag);
		}
	}
}
