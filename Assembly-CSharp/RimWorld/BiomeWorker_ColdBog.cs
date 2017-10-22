using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_ColdBog : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)((!tile.WaterCovered) ? ((!(tile.temperature < -10.0)) ? ((!(tile.swampiness < 0.5)) ? (0.0 - tile.temperature + 13.0 + tile.swampiness * 8.0) : 0.0) : 0.0) : -100.0);
		}
	}
}
