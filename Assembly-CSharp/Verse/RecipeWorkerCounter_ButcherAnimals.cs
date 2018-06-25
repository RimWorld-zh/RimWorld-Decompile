using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B29 RID: 2857
	public class RecipeWorkerCounter_ButcherAnimals : RecipeWorkerCounter
	{
		// Token: 0x06003EFC RID: 16124 RVA: 0x00212FD0 File Offset: 0x002113D0
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x00212FE8 File Offset: 0x002113E8
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

		// Token: 0x06003EFE RID: 16126 RVA: 0x00213040 File Offset: 0x00211440
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.MeatRaw.label;
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x00213060 File Offset: 0x00211460
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
