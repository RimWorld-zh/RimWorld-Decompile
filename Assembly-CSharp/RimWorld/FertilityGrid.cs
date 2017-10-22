using Verse;

namespace RimWorld
{
	public sealed class FertilityGrid
	{
		private Map map;

		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		private float CalculateFertilityAt(IntVec3 loc)
		{
			Thing edifice = loc.GetEdifice(this.map);
			return (edifice == null || !(edifice.def.fertility >= 0.0)) ? this.map.terrainGrid.TerrainAt(loc).fertility : edifice.def.fertility;
		}
	}
}
