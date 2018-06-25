using System;
using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_TemperateSwamp : BiomeWorker
	{
		public BiomeWorker_TemperateSwamp()
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
			else if (tile.rainfall < 600f)
			{
				result = 0f;
			}
			else if (tile.swampiness < 0.5f)
			{
				result = 0f;
			}
			else
			{
				result = 15f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 180f + tile.swampiness * 3f;
			}
			return result;
		}
	}
}
