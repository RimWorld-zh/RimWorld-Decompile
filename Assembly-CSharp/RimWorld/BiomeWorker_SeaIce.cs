using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_SeaIce : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			if (!tile.WaterCovered)
			{
				return -100f;
			}
			return (float)(BiomeWorker_IceSheet.PermaIceScore(tile) - 23.0);
		}
	}
}
