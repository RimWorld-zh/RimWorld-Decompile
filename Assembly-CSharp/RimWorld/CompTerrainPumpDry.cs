using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000740 RID: 1856
	public class CompTerrainPumpDry : CompTerrainPump
	{
		// Token: 0x06002912 RID: 10514 RVA: 0x0015E193 File Offset: 0x0015C593
		protected override void AffectCell(IntVec3 c)
		{
			CompTerrainPumpDry.AffectCell(this.parent.Map, c);
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x0015E1A8 File Offset: 0x0015C5A8
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

		// Token: 0x06002914 RID: 10516 RVA: 0x0015E20C File Offset: 0x0015C60C
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
