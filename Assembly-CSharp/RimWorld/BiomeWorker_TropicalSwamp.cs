using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_TropicalSwamp : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)((!tile.WaterCovered) ? ((!(tile.temperature < 15.0)) ? ((!(tile.rainfall < 2000.0)) ? ((!(tile.swampiness < 0.5)) ? (28.0 + (tile.temperature - 20.0) * 1.5 + (tile.rainfall - 600.0) / 165.0 + tile.swampiness * 3.0) : 0.0) : 0.0) : 0.0) : -100.0);
		}
	}
}
