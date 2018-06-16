using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C1 RID: 1473
	public class WorldGenStep_Features : WorldGenStep
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x000F2B50 File Offset: 0x000F0F50
		public override int SeedPart
		{
			get
			{
				return 711240483;
			}
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x000F2B6C File Offset: 0x000F0F6C
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
	}
}
