using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_TropicalSwamp : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < 15.0)
			{
				return 0f;
			}
			if (tile.rainfall < 2000.0)
			{
				return 0f;
			}
			if (tile.swampiness < 0.5)
			{
				return 0f;
			}
			return (float)(28.0 + (tile.temperature - 20.0) * 1.5 + (tile.rainfall - 600.0) / 165.0 + tile.swampiness * 3.0);
		}
	}
}
