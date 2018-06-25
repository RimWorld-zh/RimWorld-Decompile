using System;
using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_ColdBog : BiomeWorker
	{
		public BiomeWorker_ColdBog()
		{
		}

		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else if (tile.temperature < -10f)
			{
				result = 0f;
			}
			else if (tile.swampiness < 0.5f)
			{
				result = 0f;
			}
			else
			{
				result = -tile.temperature + 13f + tile.swampiness * 8f;
			}
			return result;
		}
	}
}
