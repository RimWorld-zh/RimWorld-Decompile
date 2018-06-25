using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B29 RID: 2857
	public class RecipeWorkerCounter_MakeStoneBlocks : RecipeWorkerCounter
	{
		// Token: 0x06003EF7 RID: 16119 RVA: 0x00213170 File Offset: 0x00211570
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x00213188 File Offset: 0x00211588
		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = ThingCategoryDefOf.StoneBlocks.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x002131E0 File Offset: 0x002115E0
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.StoneBlocks.label;
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x00213200 File Offset: 0x00211600
		public override bool CanPossiblyStoreInStockpile(Bill_Production bill, Zone_Stockpile stockpile)
		{
			foreach (ThingDef thingDef in bill.ingredientFilter.AllowedThingDefs)
			{
				if (!thingDef.butcherProducts.NullOrEmpty<ThingDefCountClass>())
				{
					ThingDef thingDef2 = thingDef.butcherProducts[0].thingDef;
					if (!stockpile.GetStoreSettings().AllowedToAccept(thingDef2))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
