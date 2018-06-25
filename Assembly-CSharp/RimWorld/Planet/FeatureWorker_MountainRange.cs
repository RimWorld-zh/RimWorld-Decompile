using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056A RID: 1386
	public class FeatureWorker_MountainRange : FeatureWorker_Cluster
	{
		// Token: 0x06001A3C RID: 6716 RVA: 0x000E3728 File Offset: 0x000E1B28
		protected override bool IsRoot(int tile)
		{
			return Find.WorldGrid[tile].hilliness != Hilliness.Flat;
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x000E3754 File Offset: 0x000E1B54
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].biome != BiomeDefOf.Ocean;
		}
	}
}
