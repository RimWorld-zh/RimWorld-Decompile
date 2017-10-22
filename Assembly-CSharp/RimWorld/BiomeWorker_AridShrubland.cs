using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_AridShrubland : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)((!tile.WaterCovered) ? ((!(tile.temperature < -10.0)) ? ((!(tile.rainfall < 600.0) && !(tile.rainfall >= 2000.0)) ? (22.5 + (tile.temperature - 20.0) * 2.2000000476837158 + (tile.rainfall - 600.0) / 100.0) : 0.0) : 0.0) : -100.0);
		}
	}
}
