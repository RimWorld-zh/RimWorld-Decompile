using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000568 RID: 1384
	public class FeatureWorker_Archipelago : FeatureWorker_Cluster
	{
		// Token: 0x06001A29 RID: 6697 RVA: 0x000E36A0 File Offset: 0x000E1AA0
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x000E36E0 File Offset: 0x000E1AE0
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			return true;
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x000E36FC File Offset: 0x000E1AFC
		protected override bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			bool flag;
			return base.IsMember(tile, out flag);
		}
	}
}
