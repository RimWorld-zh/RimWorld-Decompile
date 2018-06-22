using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000489 RID: 1161
	public static class PawnInventoryGenerator
	{
		// Token: 0x06001482 RID: 5250 RVA: 0x000B4298 File Offset: 0x000B2698
		public static void GenerateInventoryFor(Pawn p, PawnGenerationRequest request)
		{
			p.inventory.DestroyAll(DestroyMode.Vanish);
			for (int i = 0; i < p.kindDef.fixedInventory.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = p.kindDef.fixedInventory[i];
				Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
				thing.stackCount = thingDefCountClass.count;
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

		// Token: 0x06001483 RID: 5251 RVA: 0x000B43B0 File Offset: 0x000B27B0
		public static void GiveRandomFood(Pawn p)
		{
			if (p.kindDef.invNutrition > 0.001f)
			{
				ThingDef def;
				if (p.kindDef.invFoodDef != null)
				{
					def = p.kindDef.invFoodDef;
				}
				else
				{
					float value = Rand.Value;
					if (value < 0.5f)
					{
						def = ThingDefOf.MealSimple;
					}
					else if ((double)value < 0.75)
					{
						def = ThingDefOf.MealFine;
					}
					else
					{
						def = ThingDefOf.MealSurvivalPack;
					}
				}
				Thing thing = ThingMaker.MakeThing(def, null);
				thing.stackCount = GenMath.RoundRandom(p.kindDef.invNutrition / thing.GetStatValue(StatDefOf.Nutrition, true));
				p.inventory.TryAddItemNotForSale(thing);
			}
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x000B446C File Offset: 0x000B286C
		private static void GiveDrugsIfAddicted(Pawn p)
		{
			if (p.RaceProps.Humanlike)
			{
				IEnumerable<Hediff_Addiction> hediffs = p.health.hediffSet.GetHediffs<Hediff_Addiction>();
				using (IEnumerator<Hediff_Addiction> enumerator = hediffs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Hediff_Addiction addiction = enumerator.Current;
						IEnumerable<ThingDef> source = DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef x)
						{
							bool result;
							if (x.category != ThingCategory.Item)
							{
								result = false;
							}
							else if (p.Faction != null && x.techLevel > p.Faction.def.techLevel)
							{
								result = false;
							}
							else
							{
								CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
								result = (compProperties != null && compProperties.chemical != null && compProperties.chemical.addictionHediff == addiction.def);
							}
							return result;
						});
						ThingDef def;
						if (source.TryRandomElement(out def))
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

		// Token: 0x06001485 RID: 5253 RVA: 0x000B4568 File Offset: 0x000B2968
		private static void GiveCombatEnhancingDrugs(Pawn pawn)
		{
			if (Rand.Value < pawn.kindDef.combatEnhancingDrugsChance)
			{
				if (!pawn.IsTeetotaler())
				{
					for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
					{
						CompDrug compDrug = pawn.inventory.innerContainer[i].TryGetComp<CompDrug>();
						if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
						{
							return;
						}
					}
					int randomInRange = pawn.kindDef.combatEnhancingDrugsCount.RandomInRange;
					if (randomInRange > 0)
					{
						IEnumerable<ThingDef> source = DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef x)
						{
							bool result;
							if (x.category != ThingCategory.Item)
							{
								result = false;
							}
							else if (pawn.Faction != null && x.techLevel > pawn.Faction.def.techLevel)
							{
								result = false;
							}
							else
							{
								CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
								result = (compProperties != null && compProperties.isCombatEnhancingDrug);
							}
							return result;
						});
						for (int j = 0; j < randomInRange; j++)
						{
							ThingDef def;
							if (!source.TryRandomElement(out def))
							{
								break;
							}
							pawn.inventory.innerContainer.TryAdd(ThingMaker.MakeThing(def, null), true);
						}
					}
				}
			}
		}
	}
}
