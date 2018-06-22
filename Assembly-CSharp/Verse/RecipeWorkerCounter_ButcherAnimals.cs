using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B27 RID: 2855
	public class RecipeWorkerCounter_ButcherAnimals : RecipeWorkerCounter
	{
		// Token: 0x06003EF8 RID: 16120 RVA: 0x00212EA4 File Offset: 0x002112A4
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x00212EBC File Offset: 0x002112BC
		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = ThingCategoryDefOf.MeatRaw.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x00212F14 File Offset: 0x00211314
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.MeatRaw.label;
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x00212F34 File Offset: 0x00211334
		public override bool CanPossiblyStoreInStockpile(Bill_Production bill, Zone_Stockpile stockpile)
		{
			foreach (ThingDef thingDef in bill.ingredientFilter.AllowedThingDefs)
			{
				if (thingDef.ingestible != null && thingDef.ingestible.sourceDef != null)
				{
					RaceProperties race = thingDef.ingestible.sourceDef.race;
					if (race != null && race.meatDef != null)
					{
						if (!stockpile.GetStoreSettings().AllowedToAccept(race.meatDef))
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}
}
