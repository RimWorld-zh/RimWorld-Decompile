using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000568 RID: 1384
	public class FeatureWorker_MountainRange : FeatureWorker_Cluster
	{
		// Token: 0x06001A38 RID: 6712 RVA: 0x000E35D8 File Offset: 0x000E19D8
		protected override bool IsRoot(int tile)
		{
			return Find.WorldGrid[tile].hilliness != Hilliness.Flat;
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x000E3604 File Offset: 0x000E1A04
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].biome != BiomeDefOf.Ocean;
		}
	}
}
