using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F2 RID: 1010
	public class RoadDefGenStep_DryWithFallback : RoadDefGenStep
	{
		// Token: 0x06001167 RID: 4455 RVA: 0x00096E89 File Offset: 0x00095289
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, this.fallback);
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00096E9C File Offset: 0x0009529C
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

		// Token: 0x04000A99 RID: 2713
		public TerrainDef fallback;
	}
}
