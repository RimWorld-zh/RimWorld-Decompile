using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	public class WorldGenStep_Features : WorldGenStep
	{
		public override void GenerateFresh(string seed)
		{
			Rand.Seed = GenText.StableStringHash(seed);
			Find.World.features = new WorldFeatures();
			IOrderedEnumerable<FeatureDef> orderedEnumerable = from x in DefDatabase<FeatureDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			foreach (FeatureDef item in orderedEnumerable)
			{
				try
				{
					item.Worker.GenerateWhereAppropriate();
				}
				catch (Exception ex)
				{
					Log.Error("Could not generate world features of def " + item + ": " + ex);
				}
			}
			Rand.RandomizeStateFromTime();
		}
	}
}
