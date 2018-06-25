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
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else
			{
				result = -tile.temperature;
			}
			return result;
		}
	}
}
