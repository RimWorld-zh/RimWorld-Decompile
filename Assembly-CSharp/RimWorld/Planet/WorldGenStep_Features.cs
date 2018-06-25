using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class WorldGenStep_Features : WorldGenStep
	{
		[CompilerGenerated]
		private static Func<FeatureDef, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<FeatureDef, ushort> <>f__am$cache1;

		public WorldGenStep_Features()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 711240483;
			}
		}

		public override void GenerateFresh(string seed)
		{
			Find.World.features = new WorldFeatures();
			IOrderedEnumerable<FeatureDef> orderedEnumerable = from x in DefDatabase<FeatureDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			foreach (FeatureDef featureDef in orderedEnumerable)
			{
				try
				{
					featureDef.Worker.GenerateWhereAppropriate();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not generate world features of def ",
						featureDef,
						": ",
						ex
					}), false);
				}
			}
		}

		[CompilerGenerated]
		private static float <GenerateFresh>m__0(FeatureDef x)
		{
			return x.order;
		}

		[CompilerGenerated]
		private static ushort <GenerateFresh>m__1(FeatureDef x)
		{
			return x.index;
		}
	}
}
