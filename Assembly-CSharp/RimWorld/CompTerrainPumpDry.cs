using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000744 RID: 1860
	public class CompTerrainPumpDry : CompTerrainPump
	{
		// Token: 0x06002919 RID: 10521 RVA: 0x0015DFBB File Offset: 0x0015C3BB
		protected override void AffectCell(IntVec3 c)
		{
			CompTerrainPumpDry.AffectCell(this.parent.Map, c);
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x0015DFD0 File Offset: 0x0015C3D0
		public static void AffectCell(Map map, IntVec3 c)
		{
			TerrainDef terrain = c.GetTerrain(map);
			TerrainDef terrainToDryTo = CompTerrainPumpDry.GetTerrainToDryTo(map, terrain);
			if (terrainToDryTo != null)
			{
				map.terrainGrid.SetTerrain(c, terrainToDryTo);
			}
			TerrainDef terrainDef = map.terrainGrid.UnderTerrainAt(c);
			if (terrainDef != null)
			{
				TerrainDef terrainToDryTo2 = CompTerrainPumpDry.GetTerrainToDryTo(map, terrainDef);
				if (terrainToDryTo2 != null)
				{
					map.terrainGrid.SetUnderTerrain(c, terrainToDryTo2);
				}
			}
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x0015E034 File Offset: 0x0015C434
		private static TerrainDef GetTerrainToDryTo(Map map, TerrainDef terrainDef)
		{
			TerrainDef result;
			if (terrainDef.driesTo == null)
			{
				result = null;
			}
			else if (map.Biome == BiomeDefOf.SeaIce)
			{
				result = TerrainDefOf.Ice;
			}
			else
			{
				result = terrainDef.driesTo;
			}
			return result;
		}
	}
}
