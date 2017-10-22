using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)((!tile.WaterCovered) ? ((!(tile.rainfall >= 340.0)) ? (tile.temperature * 2.7000000476837158 - 13.0 - tile.rainfall * 0.14000000059604645) : 0.0) : -100.0);
		}
	}
}
