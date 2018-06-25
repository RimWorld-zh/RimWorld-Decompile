using System;
using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		public BiomeWorker_ExtremeDesert()
		{
		}

		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else if (tile.rainfall >= 340f)
			{
				result = 0f;
			}
			else
			{
				result = tile.temperature * 2.7f - 13f - tile.rainfall * 0.14f;
			}
			return result;
		}
	}
}
