using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_Tundra : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)((!tile.WaterCovered) ? (0.0 - tile.temperature) : -100.0);
		}
	}
}
