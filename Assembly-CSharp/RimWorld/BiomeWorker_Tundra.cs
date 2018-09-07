using System;
using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_Tundra : BiomeWorker
	{
		public BiomeWorker_Tundra()
		{
		}

		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return -tile.temperature;
		}
	}
}
