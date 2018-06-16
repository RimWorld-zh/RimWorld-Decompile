using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B29 RID: 2857
	public class RecipeWorkerCounter
	{
		// Token: 0x06003EEE RID: 16110 RVA: 0x00212390 File Offset: 0x00210790
		public virtual bool CanCountProducts(Bill_Production bill)
		{
			return this.recipe.specialProducts == null && this.recipe.products != null && this.recipe.products.Count == 1;
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x002123DC File Offset: 0x002107DC
		public virtual int CountProducts(Bill_Production bill)
		{
			ThingDefCountClass thingDefCountClass = this.recipe.products[0];
			ThingDef prodDef = thingDefCountClass.thingDef;
			int result;
			if (thingDefCountClass.thingDef.CountAsResource && !bill.includeEquipped && (bill.includeTainted || !thingDefCountClass.thingDef.IsApparel || !thingDefCountClass.thingDef.apparel.careIfWornByCorpse) && bill.includeFromZone == null && bill.hpRange.min == 0f && bill.hpRange.max == 1f && bill.qualityRange.min == QualityCategory.Awful && bill.qualityRange.max == QualityCategory.Legendary && !bill.limitToAllowedStuff)
			{
				result = bill.Map.resourceCounter.GetCount(thingDefCountClass.thingDef);
			}
			else
			{
				int num = 0;
				if (bill.includeFromZone == null)
				{
					num = this.CountValidThings(bill.Map.listerThings.ThingsOfDef(thingDefCountClass.thingDef), bill, prodDef);
					if (thingDefCountClass.thingDef.Minifiable)
					{
						List<Thing> list = bill.Map.listerThings.ThingsInGroup(ThingRequestGroup.MinifiedThing);
						for (int i = 0; i < list.Count; i++)
						{
							MinifiedThing minifiedThing = (MinifiedThing)list[i];
							if (this.CountValidThing(minifiedThing.InnerThing, bill, prodDef))
							{
								num += minifiedThing.stackCount * minifiedThing.InnerThing.stackCount;
							}
						}
					}
				}
				else
				{
					foreach (Thing outerThing in bill.includeFromZone.AllContainedThings)
					{
						Thing innerIfMinified = outerThing.GetInnerIfMinified();
						if (this.CountValidThing(innerIfMinified, bill, prodDef))
						{
							num += innerIfMinified.stackCount;
						}
					}
				}
				if (bill.includeEquipped)
				{
					foreach (Pawn pawn in bill.Map.mapPawns.FreeColonistsSpawned)
					{
						num += pawn.equipment.AllEquipmentListForReading.Count((ThingWithComps thing) => this.CountValidThing(thing, bill, prodDef));
						num += pawn.apparel.WornApparel.Count((Apparel thing) => this.CountValidThing(thing, bill, prodDef));
						num += pawn.inventory.GetDirectlyHeldThings().Count((Thing thing) => this.CountValidThing(thing, bill, prodDef));
					}
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x00212740 File Offset: 0x00210B40
		public int CountValidThings(List<Thing> things, Bill_Production bill, ThingDef def)
		{
			int num = 0;
			for (int i = 0; i < things.Count; i++)
			{
				if (this.CountValidThing(things[i], bill, def))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x0021278C File Offset: 0x00210B8C
		public bool CountValidThing(Thing thing, Bill_Production bill, ThingDef def)
		{
			ThingDef def2 = thing.def;
			bool result;
			if (def2 != def)
			{
				result = false;
			}
			else if (!bill.includeTainted && def2.IsApparel && (thing as Apparel).WornByCorpse)
			{
				result = false;
			}
			else if (!bill.hpRange.IncludesEpsilon((float)thing.HitPoints / (float)thing.MaxHitPoints))
			{
				result = false;
			}
			else
			{
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				result = ((compQuality == null || bill.qualityRange.Includes(compQuality.Quality)) && (!bill.limitToAllowedStuff || bill.ingredientFilter.Allows(thing.Stuff)));
			}
			return result;
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x0021285C File Offset: 0x00210C5C
		public virtual string ProductsDescription(Bill_Production bill)
		{
			return null;
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x00212874 File Offset: 0x00210C74
		public virtual bool CanPossiblyStoreInStockpile(Bill_Production bill, Zone_Stockpile stockpile)
		{
			return !this.CanCountProducts(bill) || stockpile.GetStoreSettings().AllowedToAccept(this.recipe.products[0].thingDef);
		}

		// Token: 0x040028C9 RID: 10441
		public RecipeDef recipe;
	}
}
