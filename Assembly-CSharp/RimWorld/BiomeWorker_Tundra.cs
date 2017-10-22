using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_Tundra : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return (float)(0.0 - tile.temperature);
		}
	}
}
