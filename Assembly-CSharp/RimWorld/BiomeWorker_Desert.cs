using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_Desert : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.rainfall >= 600.0)
			{
				return 0f;
			}
			return (float)(tile.temperature + 9.9999997473787516E-05);
		}
	}
}
