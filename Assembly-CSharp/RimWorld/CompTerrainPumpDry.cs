using Verse;

namespace RimWorld
{
	public class CompTerrainPumpDry : CompTerrainPump
	{
		protected override void AffectCell(IntVec3 c)
		{
			TerrainDef terrain = c.GetTerrain(base.parent.Map);
			if (terrain.driesTo != null)
			{
				if (base.parent.Map.Biome == BiomeDefOf.SeaIce)
				{
					base.parent.Map.terrainGrid.SetTerrain(c, TerrainDefOf.Ice);
				}
				else
				{
					base.parent.Map.terrainGrid.SetTerrain(c, terrain.driesTo);
				}
			}
		}
	}
}
