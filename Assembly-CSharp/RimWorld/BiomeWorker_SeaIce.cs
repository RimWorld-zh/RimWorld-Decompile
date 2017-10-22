using RimWorld.Planet;

namespace RimWorld
{
	public class BiomeWorker_SeaIce : BiomeWorker
	{
		public override float GetScore(Tile tile)
		{
			return (float)(tile.WaterCovered ? (BiomeWorker_IceSheet.PermaIceScore(tile) - 23.0) : -100.0);
		}
	}
}
