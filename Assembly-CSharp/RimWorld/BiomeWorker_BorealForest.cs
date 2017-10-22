using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_BorealForest : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)((!tile.WaterCovered) ? ((!(tile.temperature < -10.0)) ? ((!(tile.rainfall < 600.0)) ? 15.0 : 0.0) : 0.0) : -100.0);
		}
	}
}
