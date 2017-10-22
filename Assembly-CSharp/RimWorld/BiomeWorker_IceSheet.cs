using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)((!tile.WaterCovered) ? BiomeWorker_IceSheet.PermaIceScore(tile) : -100.0);
		}

		public static float PermaIceScore(Tile tile)
		{
			return (float)(-20.0 + (0.0 - tile.temperature) * 2.0);
		}
	}
}
