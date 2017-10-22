using Verse;

namespace RimWorld.Planet
{
	public class WorldGenStep_AncientSites : WorldGenStep
	{
		public FloatRange ancientSitesPer100kTiles;

		public override void GenerateFresh(string seed)
		{
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateAncientSites();
			Rand.RandomizeStateFromTime();
		}

		public override void GenerateFromScribe(string seed)
		{
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateAncientSites();
			Rand.RandomizeStateFromTime();
		}

		private void GenerateAncientSites()
		{
			int num = GenMath.RoundRandom((float)((float)Find.WorldGrid.TilesCount / 100000.0 * this.ancientSitesPer100kTiles.RandomInRange));
			for (int num2 = 0; num2 < num; num2++)
			{
				Find.World.genData.ancientSites.Add(TileFinder.RandomFactionBaseTileFor(null, false));
			}
		}
	}
}
