using System;
using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_TropicalRainforest : BiomeWorker
	{
		public BiomeWorker_TropicalRainforest()
		{
		}

		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < 15f)
			{
				return 0f;
			}
			if (tile.rainfall < 2000f)
			{
				return 0f;
			}
			return 28f + (tile.temperature - 20f) * 1.5f + (tile.rainfall - 600f) / 165f;
		}
	}
}
