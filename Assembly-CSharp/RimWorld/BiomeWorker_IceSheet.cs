using System;
using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		public BiomeWorker_IceSheet()
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
				result = BiomeWorker_IceSheet.PermaIceScore(tile);
			}
			return result;
		}

		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
