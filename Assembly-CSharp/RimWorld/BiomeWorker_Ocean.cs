using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_Ocean : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)(tile.WaterCovered ? 0.0 : -100.0);
		}
	}
}
