using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.rainfall >= 340.0)
			{
				return 0f;
			}
			return (float)(tile.temperature * 2.7000000476837158 - 13.0 - tile.rainfall * 0.14000000059604645);
		}
	}
}
