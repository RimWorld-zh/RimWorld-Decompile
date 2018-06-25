using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000742 RID: 1858
	public class CompTerrainPumpDry : CompTerrainPump
	{
		// Token: 0x06002916 RID: 10518 RVA: 0x0015E2E3 File Offset: 0x0015C6E3
		protected override void AffectCell(IntVec3 c)
		{
			CompTerrainPumpDry.AffectCell(this.parent.Map, c);
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x0015E2F8 File Offset: 0x0015C6F8
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

		// Token: 0x06002918 RID: 10520 RVA: 0x0015E35C File Offset: 0x0015C75C
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
