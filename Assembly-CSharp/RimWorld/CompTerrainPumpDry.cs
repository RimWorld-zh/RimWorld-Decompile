using Verse;

namespace RimWorld
{
	public class CompTerrainPumpDry : CompTerrainPump
	{
		protected override void AffectCell(IntVec3 c)
		{
			CompTerrainPumpDry.AffectCell(base.parent.Map, c);
		}

		public static void AffectCell(Map map, IntVec3 c)
		{
			TerrainDef terrain = c.GetTerrain(map);
			if (terrain.driesTo != null)
			{
				if (map.Biome == BiomeDefOf.SeaIce)
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.Ice);
				}
				else
				{
					map.terrainGrid.SetTerrain(c, terrain.driesTo);
				}
			}
		}
	}
}
