using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B26 RID: 2854
	public class RecipeWorkerCounter_MakeStoneBlocks : RecipeWorkerCounter
	{
		// Token: 0x06003EF3 RID: 16115 RVA: 0x00212D64 File Offset: 0x00211164
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x00212D7C File Offset: 0x0021117C
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

		// Token: 0x06003EF5 RID: 16117 RVA: 0x00212DD4 File Offset: 0x002111D4
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.StoneBlocks.label;
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x00212DF4 File Offset: 0x002111F4
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
