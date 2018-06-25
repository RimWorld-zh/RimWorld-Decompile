using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000568 RID: 1384
	public class FeatureWorker_Archipelago : FeatureWorker_Cluster
	{
		// Token: 0x06001A28 RID: 6696 RVA: 0x000E3908 File Offset: 0x000E1D08
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x000E3948 File Offset: 0x000E1D48
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			return true;
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x000E3964 File Offset: 0x000E1D64
		protected override bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			bool flag;
			return base.IsMember(tile, out flag);
		}
	}
}
