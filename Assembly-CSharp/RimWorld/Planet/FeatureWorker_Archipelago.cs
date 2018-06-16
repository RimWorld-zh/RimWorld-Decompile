using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056A RID: 1386
	public class FeatureWorker_Archipelago : FeatureWorker_Cluster
	{
		// Token: 0x06001A2D RID: 6701 RVA: 0x000E34A8 File Offset: 0x000E18A8
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x000E34E8 File Offset: 0x000E18E8
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			return true;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x000E3504 File Offset: 0x000E1904
		protected override bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			bool flag;
			return base.IsMember(tile, out flag);
		}
	}
}
