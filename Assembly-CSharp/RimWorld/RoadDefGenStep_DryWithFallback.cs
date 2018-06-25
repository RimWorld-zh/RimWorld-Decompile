using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F4 RID: 1012
	public class RoadDefGenStep_DryWithFallback : RoadDefGenStep
	{
		// Token: 0x04000A9B RID: 2715
		public TerrainDef fallback;

		// Token: 0x0600116B RID: 4459 RVA: 0x000971C5 File Offset: 0x000955C5
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, this.fallback);
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x000971D8 File Offset: 0x000955D8
		public static void PlaceWorker(Map map, IntVec3 position, TerrainDef fallback)
		{
			while (map.terrainGrid.TerrainAt(position).driesTo != null)
			{
				map.terrainGrid.SetTerrain(position, map.terrainGrid.TerrainAt(position).driesTo);
			}
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(position);
			if (terrainDef.passability == Traversability.Impassable || terrainDef.IsRiver)
			{
				map.terrainGrid.SetTerrain(position, fallback);
			}
		}
	}
}
