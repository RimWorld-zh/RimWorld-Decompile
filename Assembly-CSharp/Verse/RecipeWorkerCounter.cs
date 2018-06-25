using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

		public virtual string ProductsDescription(Bill_Production bill)
		{
			return null;
		}

		public virtual bool CanPossiblyStoreInStockpile(Bill_Production bill, Zone_Stockpile stockpile)
		{
			return !this.CanCountProducts(bill) || stockpile.GetStoreSettings().AllowedToAccept(this.recipe.products[0].thingDef);
		}

		[CompilerGenerated]
		private sealed class <CountProducts>c__AnonStorey0
		{
			internal Bill_Production bill;

			internal ThingDef prodDef;

			internal RecipeWorkerCounter $this;

			public <CountProducts>c__AnonStorey0()
			{
			}

			internal bool <>m__0(ThingWithComps thing)
			{
				return this.$this.CountValidThing(thing, this.bill, this.prodDef);
			}

			internal bool <>m__1(Apparel thing)
			{
				return this.$this.CountValidThing(thing, this.bill, this.prodDef);
			}

			internal bool <>m__2(Thing thing)
			{
				return this.$this.CountValidThing(thing, this.bill, this.prodDef);
			}
		}
	}
}
