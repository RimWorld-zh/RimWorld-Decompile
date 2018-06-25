using System;
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
			float fertility;
			if (edifice != null && edifice.def.fertility >= 0f)
			{
				fertility = edifice.def.fertility;
			}
			else
			{
				fertility = this.map.terrainGrid.TerrainAt(loc).fertility;
			}
			return fertility;
		}
	}
}
