using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056C RID: 1388
	public class FeatureWorker_MountainRange : FeatureWorker_Cluster
	{
		// Token: 0x06001A41 RID: 6721 RVA: 0x000E3584 File Offset: 0x000E1984
		protected override bool IsRoot(int tile)
		{
			return Find.WorldGrid[tile].hilliness != Hilliness.Flat;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x000E35B0 File Offset: 0x000E19B0
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].biome != BiomeDefOf.Ocean;
		}
	}
}
