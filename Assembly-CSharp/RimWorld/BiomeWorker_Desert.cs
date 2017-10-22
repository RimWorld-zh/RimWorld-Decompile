using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_Desert : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)((!tile.WaterCovered) ? ((!(tile.rainfall >= 600.0)) ? (tile.temperature + 9.9999997473787516E-05) : 0.0) : -100.0);
		}
	}
}
