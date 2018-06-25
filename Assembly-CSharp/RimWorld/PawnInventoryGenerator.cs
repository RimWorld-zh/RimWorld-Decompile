using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

		[CompilerGenerated]
		private sealed class <GiveDrugsIfAddicted>c__AnonStorey0
		{
			internal Pawn p;

			public <GiveDrugsIfAddicted>c__AnonStorey0()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <GiveDrugsIfAddicted>c__AnonStorey1
		{
			internal Hediff_Addiction addiction;

			internal PawnInventoryGenerator.<GiveDrugsIfAddicted>c__AnonStorey0 <>f__ref$0;

			public <GiveDrugsIfAddicted>c__AnonStorey1()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				bool result;
				if (x.category != ThingCategory.Item)
				{
					result = false;
				}
				else if (this.<>f__ref$0.p.Faction != null && x.techLevel > this.<>f__ref$0.p.Faction.def.techLevel)
				{
					result = false;
				}
				else
				{
					CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
					result = (compProperties != null && compProperties.chemical != null && compProperties.chemical.addictionHediff == this.addiction.def);
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <GiveCombatEnhancingDrugs>c__AnonStorey2
		{
			internal Pawn pawn;

			public <GiveCombatEnhancingDrugs>c__AnonStorey2()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				bool result;
				if (x.category != ThingCategory.Item)
				{
					result = false;
				}
				else if (this.pawn.Faction != null && x.techLevel > this.pawn.Faction.def.techLevel)
				{
					result = false;
				}
				else
				{
					CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
					result = (compProperties != null && compProperties.isCombatEnhancingDrug);
				}
				return result;
			}
		}
	}
}
