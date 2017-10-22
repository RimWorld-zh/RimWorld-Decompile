using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_TemperateSwamp : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)((!tile.WaterCovered) ? ((!(tile.temperature < -10.0)) ? ((!(tile.rainfall < 600.0)) ? ((!(tile.swampiness < 0.5)) ? (15.0 + (tile.temperature - 7.0) + (tile.rainfall - 600.0) / 180.0 + tile.swampiness * 3.0) : 0.0) : 0.0) : 0.0) : -100.0);
		}
	}
}
