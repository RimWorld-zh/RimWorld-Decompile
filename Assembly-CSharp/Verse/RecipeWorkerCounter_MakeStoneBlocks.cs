using RimWorld;
using System.Collections.Generic;

namespace Verse
{
	public class RecipeWorkerCounter_MakeStoneBlocks : RecipeWorkerCounter
	{
		private List<ThingDef> stoneBlocksDefs = null;

		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		public override int CountProducts(Bill_Production bill)
		{
			if (this.stoneBlocksDefs == null)
			{
				ThingCategoryDef stoneBlocks = ThingCategoryDefOf.StoneBlocks;
				this.stoneBlocksDefs = new List<ThingDef>(16);
				foreach (ThingDef item in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (item.thingCategories != null && item.thingCategories.Contains(stoneBlocks))
					{
						this.stoneBlocksDefs.Add(item);
					}
				}
			}
			int num = 0;
			for (int i = 0; i < this.stoneBlocksDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(this.stoneBlocksDefs[i]);
			}
			return num;
		}

		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.StoneBlocks.label;
		}
	}
}
