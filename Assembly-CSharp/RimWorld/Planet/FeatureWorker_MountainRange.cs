using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056C RID: 1388
	public class FeatureWorker_MountainRange : FeatureWorker_Cluster
	{
		// Token: 0x06001A40 RID: 6720 RVA: 0x000E3530 File Offset: 0x000E1930
		protected override bool IsRoot(int tile)
		{
			return Find.WorldGrid[tile].hilliness != Hilliness.Flat;
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x000E355C File Offset: 0x000E195C
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].biome != BiomeDefOf.Ocean;
		}
	}
}
