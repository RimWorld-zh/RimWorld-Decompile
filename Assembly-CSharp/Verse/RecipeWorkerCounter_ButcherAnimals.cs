using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B2A RID: 2858
	public class RecipeWorkerCounter_ButcherAnimals : RecipeWorkerCounter
	{
		// Token: 0x06003EFC RID: 16124 RVA: 0x002132B0 File Offset: 0x002116B0
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x002132C8 File Offset: 0x002116C8
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

		// Token: 0x06003EFE RID: 16126 RVA: 0x00213320 File Offset: 0x00211720
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.MeatRaw.label;
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x00213340 File Offset: 0x00211740
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
