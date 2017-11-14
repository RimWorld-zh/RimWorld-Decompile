using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_TemperateForest : BiomeWorker
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
			return (float)(15.0 + (tile.temperature - 7.0) + (tile.rainfall - 600.0) / 180.0);
		}
	}
}
