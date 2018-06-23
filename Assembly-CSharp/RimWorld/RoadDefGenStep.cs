using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002C6 RID: 710
	public abstract class RoadDefGenStep
	{
		// Token: 0x040006FD RID: 1789
		public SimpleCurve chancePerPositionCurve;

		// Token: 0x040006FE RID: 1790
		public float antialiasingMultiplier = 1f;

		// Token: 0x040006FF RID: 1791
		public int periodicSpacing = 0;

		// Token: 0x06000BD0 RID: 3024
		public abstract void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance);
	}
}
