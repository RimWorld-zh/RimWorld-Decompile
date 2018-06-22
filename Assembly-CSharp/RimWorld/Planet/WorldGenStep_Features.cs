using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BD RID: 1469
	public class WorldGenStep_Features : WorldGenStep
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001C39 RID: 7225 RVA: 0x000F2C1C File Offset: 0x000F101C
		public override int SeedPart
		{
			get
			{
				return 711240483;
			}
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000F2C38 File Offset: 0x000F1038
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
