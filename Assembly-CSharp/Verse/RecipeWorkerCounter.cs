using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	public class RecipeWorkerCounter
	{
		public RecipeDef recipe;

		public RecipeWorkerCounter()
		{
		}

		public virtual bool CanCountProducts(Bill_Production bill)
		{
			return this.recipe.specialProducts == null && this.recipe.products != null && this.recipe.products.Count == 1;
		}

		public virtual int CountProducts(Bill_Production bill)
		{
			ThingDefCountClass thingDefCountClass = this.recipe.products[0];
			ThingDef thingDef = thingDefCountClass.thingDef;
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
					num = this.CountValidThings(bill.Map.listerThings.ThingsOfDef(thingDefCountClass.thingDef), bill, thingDef);
					if (thingDefCountClass.thingDef.Minifiable)
					{
						List<Thing> list = bill.Map.listerThings.ThingsInGroup(ThingRequestGroup.MinifiedThing);
						for (int i = 0; i < list.Count; i++)
						{
							MinifiedThing minifiedThing = (MinifiedThing)list[i];
							if (this.CountValidThing(minifiedThing.InnerThing, bill, thingDef))
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
						if (this.CountValidThing(innerIfMinified, bill, thingDef))
						{
							num += innerIfMinified.stackCount;
						}
					}
				}
				if (bill.includeEquipped)
				{
					foreach (Pawn pawn in bill.Map.mapPawns.FreeColonistsSpawned)
					{
						List<ThingWithComps> allEquipmentListForReading = pawn.equipment.AllEquipmentListForReading;
						for (int j = 0; j < allEquipmentListForReading.Count; j++)
						{
							if (this.CountValidThing(allEquipmentListForReading[j], bill, thingDef))
							{
								num += allEquipmentListForReading[j].stackCount;
							}
						}
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int k = 0; k < wornApparel.Count; k++)
						{
							if (this.CountValidThing(wornApparel[k], bill, thingDef))
							{
								num += wornApparel[k].stackCount;
							}
						}
						ThingOwner directlyHeldThings = pawn.inventory.GetDirectlyHeldThings();
						for (int l = 0; l < directlyHeldThings.Count; l++)
						{
							if (this.CountValidThing(directlyHeldThings[l], bill, thingDef))
							{
								num += directlyHeldThings[l].stackCount;
							}
						}
					}
				}
				result = num;
			}
			return result;
		}

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

		public bool CountValidThing(Thing thing, Bill_Production bill, ThingDef def)
		{
			ThingDef def2 = thing.def;
			bool result;
			if (def2 != def)
			{
				result = false;
			}
			else if (!bill.includeTainted && def2.IsApparel && ((Apparel)thing).WornByCorpse)
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

		public virtual string ProductsDescription(Bill_Production bill)
		{
			return null;
		}

		public virtual bool CanPossiblyStoreInStockpile(Bill_Production bill, Zone_Stockpile stockpile)
		{
			return !this.CanCountProducts(bill) || stockpile.GetStoreSettings().AllowedToAccept(this.recipe.products[0].thingDef);
		}
	}
}
