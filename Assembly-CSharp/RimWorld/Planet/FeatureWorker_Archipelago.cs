using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056A RID: 1386
	public class FeatureWorker_Archipelago : FeatureWorker_Cluster
	{
		// Token: 0x06001A2E RID: 6702 RVA: 0x000E34FC File Offset: 0x000E18FC
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x000E353C File Offset: 0x000E193C
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			return true;
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x000E3558 File Offset: 0x000E1958
		protected override bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			bool flag;
			return base.IsMember(tile, out flag);
		}
	}
}
