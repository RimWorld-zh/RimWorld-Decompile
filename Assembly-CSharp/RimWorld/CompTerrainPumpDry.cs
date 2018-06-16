using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000744 RID: 1860
	public class CompTerrainPumpDry : CompTerrainPump
	{
		// Token: 0x06002917 RID: 10519 RVA: 0x0015DF27 File Offset: 0x0015C327
		protected override void AffectCell(IntVec3 c)
		{
			CompTerrainPumpDry.AffectCell(this.parent.Map, c);
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x0015DF3C File Offset: 0x0015C33C
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

		// Token: 0x06002919 RID: 10521 RVA: 0x0015DFA0 File Offset: 0x0015C3A0
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
