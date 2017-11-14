using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_ColdBog : BiomeWorker
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
			if (tile.swampiness < 0.5)
			{
				return 0f;
			}
			return (float)(0.0 - tile.temperature + 13.0 + tile.swampiness * 8.0);
		}
	}
}
