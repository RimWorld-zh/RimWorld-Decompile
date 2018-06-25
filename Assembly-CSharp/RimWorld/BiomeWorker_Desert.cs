using System;
using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_Desert : BiomeWorker
	{
		public BiomeWorker_Desert()
		{
		}

		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else if (tile.rainfall >= 600f)
			{
				result = 0f;
			}
			else
			{
				result = tile.temperature + 0.0001f;
			}
			return result;
		}
	}
}
