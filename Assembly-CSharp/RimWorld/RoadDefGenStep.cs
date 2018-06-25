using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002C8 RID: 712
	public abstract class RoadDefGenStep
	{
		// Token: 0x040006FF RID: 1791
		public SimpleCurve chancePerPositionCurve;

		// Token: 0x04000700 RID: 1792
		public float antialiasingMultiplier = 1f;

		// Token: 0x04000701 RID: 1793
		public int periodicSpacing = 0;

		// Token: 0x06000BD3 RID: 3027
		public abstract void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance);
	}
}
