using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_BorealForest : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < -10.0)
			{
				return 0f;
			}
			if (tile.rainfall < 600.0)
			{
				return 0f;
			}
			return 15f;
		}
	}
}
