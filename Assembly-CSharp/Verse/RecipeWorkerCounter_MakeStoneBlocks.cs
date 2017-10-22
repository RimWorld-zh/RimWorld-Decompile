using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse
{
	public class RecipeWorkerCounter_MakeStoneBlocks : RecipeWorkerCounter
	{
		private List<ThingDef> stoneBlocksDefs;

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
				List<ThingDef>.Enumerator enumerator = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ThingDef current = enumerator.Current;
						if (current.thingCategories != null && current.thingCategories.Contains(stoneBlocks))
						{
							this.stoneBlocksDefs.Add(current);
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
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
