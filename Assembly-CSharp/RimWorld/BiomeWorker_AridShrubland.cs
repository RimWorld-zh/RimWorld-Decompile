using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_AridShrubland : BiomeWorker
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
			if (!(tile.rainfall < 600.0) && !(tile.rainfall >= 2000.0))
			{
				return (float)(22.5 + (tile.temperature - 20.0) * 2.2000000476837158 + (tile.rainfall - 600.0) / 100.0);
			}
			return 0f;
		}
	}
}
