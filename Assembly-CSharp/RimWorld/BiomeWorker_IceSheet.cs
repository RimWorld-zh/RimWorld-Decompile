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
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return BiomeWorker_IceSheet.PermaIceScore(tile);
		}

		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
