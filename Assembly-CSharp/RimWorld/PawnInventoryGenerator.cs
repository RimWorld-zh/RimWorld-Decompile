using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class PawnInventoryGenerator
	{
		public static void GenerateInventoryFor(Pawn p, PawnGenerationRequest request)
		{
			p.inventory.DestroyAll(DestroyMode.Vanish);
			for (int i = 0; i < p.kindDef.fixedInventory.Count; i++)
			{
				ThingCountClass thingCountClass = p.kindDef.fixedInventory[i];
				Thing thing = ThingMaker.MakeThing(thingCountClass.thingDef, null);
				thing.stackCount = thingCountClass.count;
				p.inventory.innerContainer.TryAdd(thing, true);
			}
			if (p.kindDef.inventoryOptions != null)
			{
				foreach (Thing item in p.kindDef.inventoryOptions.GenerateThings())
				{
					p.inventory.innerContainer.TryAdd(item, true);
				}
			}
			if (request.AllowFood)
			{
				PawnInventoryGenerator.GiveRandomFood(p);
			}
			PawnInventoryGenerator.GiveDrugsIfAddicted(p);
			PawnInventoryGenerator.GiveCombatEnhancingDrugs(p);
		}

		public static void GiveRandomFood(Pawn p)
		{
			if (p.kindDef.invNutrition > 0.0010000000474974513)
			{
				ThingDef thingDef;
				if (p.kindDef.invFoodDef != null)
				{
					thingDef = p.kindDef.invFoodDef;
				}
				else
				{
					float value = Rand.Value;
					thingDef = ((!(value < 0.5)) ? ((!((double)value < 0.75)) ? ThingDefOf.MealSurvivalPack : ThingDefOf.MealFine) : ThingDefOf.MealSimple);
				}
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = GenMath.RoundRandom(p.kindDef.invNutrition / thingDef.ingestible.nutrition);
				p.inventory.TryAddItemNotForSale(thing);
			}
		}

		private static void GiveDrugsIfAddicted(Pawn p)
		{
			if (p.RaceProps.Humanlike)
			{
				IEnumerable<Hediff_Addiction> hediffs = p.health.hediffSet.GetHediffs<Hediff_Addiction>();
				using (IEnumerator<Hediff_Addiction> enumerator = hediffs.GetEnumerator())
				{
					Hediff_Addiction addiction;
					while (enumerator.MoveNext())
					{
						addiction = enumerator.Current;
						IEnumerable<ThingDef> source = DefDatabase<ThingDef>.AllDefsListForReading.Where((Func<ThingDef, bool>)delegate(ThingDef x)
						{
							if (x.category != ThingCategory.Item)
							{
								return false;
							}
							if (p.Faction != null && (int)x.techLevel > (int)p.Faction.def.techLevel)
							{
								return false;
							}
							CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
							return compProperties != null && compProperties.chemical != null && compProperties.chemical.addictionHediff == addiction.def;
						});
						ThingDef def = default(ThingDef);
						if (source.TryRandomElement<ThingDef>(out def))
						{
							int stackCount = Rand.RangeInclusive(2, 5);
							Thing thing = ThingMaker.MakeThing(def, null);
							thing.stackCount = stackCount;
							p.inventory.TryAddItemNotForSale(thing);
						}
					}
				}
			}
		}

		private static void GiveCombatEnhancingDrugs(Pawn pawn)
		{
			if (!(Rand.Value >= pawn.kindDef.combatEnhancingDrugsChance) && !pawn.IsTeetotaler())
			{
				for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
				{
					CompDrug compDrug = pawn.inventory.innerContainer[i].TryGetComp<CompDrug>();
					if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
						return;
				}
				int randomInRange = pawn.kindDef.combatEnhancingDrugsCount.RandomInRange;
				if (randomInRange > 0)
				{
					IEnumerable<ThingDef> source = DefDatabase<ThingDef>.AllDefsListForReading.Where((Func<ThingDef, bool>)delegate(ThingDef x)
					{
						if (x.category != ThingCategory.Item)
						{
							return false;
						}
						if (pawn.Faction != null && (int)x.techLevel > (int)pawn.Faction.def.techLevel)
						{
							return false;
						}
						CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
						if (compProperties != null && compProperties.isCombatEnhancingDrug)
						{
							return true;
						}
						return false;
					});
					int num = 0;
					ThingDef def = default(ThingDef);
					while (num < randomInRange && source.TryRandomElement<ThingDef>(out def))
					{
						pawn.inventory.innerContainer.TryAdd(ThingMaker.MakeThing(def, null), true);
						num++;
					}
				}
			}
		}
	}
}
