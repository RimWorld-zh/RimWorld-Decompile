using System;
using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_Ocean : BiomeWorker
	{
		public BiomeWorker_Ocean()
		{
		}

		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (!tile.WaterCovered)
			{
				result = -100f;
			}
			else
			{
				result = 0f;
			}
			return result;
		}
	}
}
