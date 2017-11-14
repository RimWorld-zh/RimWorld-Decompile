using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Verse
{
	internal static class DataAnalysisTableMaker
	{
		[CompilerGenerated]
		private static Func<float, string> _003C_003Ef__mg_0024cache0;

		public static void DoTable_PopulationIntent()
		{
			Find.Storyteller.intenderPopulation.DoTable_PopulationIntents();
		}

		public static void DoTable_PopAdjRecruitDifficulty()
		{
			PawnUtility.DoTable_PopIntentRecruitDifficulty();
		}

		public static void DoTable_ManhunterResults()
		{
			ManhunterPackIncidentUtility.DoTable_ManhunterResults();
		}

		public static void DoTable_DrugEconomy()
		{
			Func<ThingDef, string> ingredients = delegate(ThingDef d)
			{
				if (d.costList == null)
				{
					return "-";
				}
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ThingCountClass cost in d.costList)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					string text = (!DataAnalysisTableMaker.RequiresBuying(cost.thingDef)) ? string.Empty : "*";
					stringBuilder.Append(cost.thingDef.defName + text + " x" + cost.count);
				}
				return stringBuilder.ToString().TrimEndNewlines();
			};
			Func<ThingDef, float> workAmount = delegate(ThingDef d)
			{
				if (d.recipeMaker == null)
				{
					return -1f;
				}
				if (d.recipeMaker.workAmount >= 0)
				{
					return (float)d.recipeMaker.workAmount;
				}
				return Mathf.Max(d.GetStatValueAbstract(StatDefOf.WorkToMake, null), d.GetStatValueAbstract(StatDefOf.WorkToBuild, null));
			};
			Func<ThingDef, float> realIngredientCost = (ThingDef d) => DataAnalysisTableMaker.CostToMake(d, true);
			Func<ThingDef, float> realSellPrice = (ThingDef d) => (float)(d.BaseMarketValue * 0.5);
			Func<ThingDef, float> realBuyPrice = (ThingDef d) => (float)(d.BaseMarketValue * 1.5);
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.IsWithinCategory(ThingCategoryDefOf.Medicine) || d.IsWithinCategory(ThingCategoryDefOf.Drugs)
			select d, new TableDataGetter<ThingDef>("name", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("ingredients", (ThingDef d) => ingredients(d)), new TableDataGetter<ThingDef>("work amount", (ThingDef d) => workAmount(d).ToString("F0")), new TableDataGetter<ThingDef>("real ingredient cost", (ThingDef d) => realIngredientCost(d).ToString("F1")), new TableDataGetter<ThingDef>("real sell price", (ThingDef d) => realSellPrice(d).ToString("F1")), new TableDataGetter<ThingDef>("real profit per item", (ThingDef d) => (realSellPrice(d) - realIngredientCost(d)).ToString("F1")), new TableDataGetter<ThingDef>("real profit per day's work", (ThingDef d) => ((float)((realSellPrice(d) - realIngredientCost(d)) / workAmount(d) * 30000.0)).ToString("F1")), new TableDataGetter<ThingDef>("real buy price", (ThingDef d) => realBuyPrice(d).ToString("F1")));
		}

		public static void DoTable_WoolEconomy()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh && d.GetCompProperties<CompProperties_Shearable>() != null
			select d, new TableDataGetter<ThingDef>("animal", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("woolDef", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolDef.defName), new TableDataGetter<ThingDef>("woolAmount", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolAmount.ToString()), new TableDataGetter<ThingDef>("woolValue", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolDef.BaseMarketValue.ToString("F2")), new TableDataGetter<ThingDef>("shear interval", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().shearIntervalDays.ToString("F1")), new TableDataGetter<ThingDef>("value per year", delegate(ThingDef d)
			{
				CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
				return ((float)(compProperties.woolDef.BaseMarketValue * (float)compProperties.woolAmount * (60.0 / (float)compProperties.shearIntervalDays))).ToString("F0");
			}));
		}

		public static void DoTable_AnimalGrowthEconomy()
		{
			Func<ThingDef, float> gestDays = delegate(ThingDef d)
			{
				if (d.HasComp(typeof(CompEggLayer)))
				{
					CompProperties_EggLayer compProperties2 = d.GetCompProperties<CompProperties_EggLayer>();
					return compProperties2.eggLayIntervalDays / compProperties2.eggCountRange.Average;
				}
				return d.race.gestationPeriodDays;
			};
			Func<ThingDef, float> nutritionToGestate = delegate(ThingDef d)
			{
				float num4 = 0f;
				LifeStageAge lifeStageAge3 = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1];
				return num4 + gestDays(d) * lifeStageAge3.def.hungerRateFactor * d.race.baseHungerRate;
			};
			Func<ThingDef, float> babyMeatNut = delegate(ThingDef d)
			{
				LifeStageAge lifeStageAge2 = d.race.lifeStageAges[0];
				return (float)(d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05000000074505806 * lifeStageAge2.def.bodySizeFactor);
			};
			Func<ThingDef, float> babyMeatNutPerInput = (ThingDef d) => babyMeatNut(d) / nutritionToGestate(d);
			Func<ThingDef, float> nutritionToAdulthood = delegate(ThingDef d)
			{
				float num = 0f;
				num += nutritionToGestate(d);
				for (int i = 1; i < d.race.lifeStageAges.Count; i++)
				{
					LifeStageAge lifeStageAge = d.race.lifeStageAges[i];
					float num2 = lifeStageAge.minAge - d.race.lifeStageAges[i - 1].minAge;
					float num3 = (float)(num2 * 60.0);
					num += num3 * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
				}
				return num;
			};
			Func<ThingDef, float> adultMeatNutPerInput = (ThingDef d) => (float)(d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05000000074505806 / nutritionToAdulthood(d));
			Func<ThingDef, float> bestMeatPerInput = delegate(ThingDef d)
			{
				float a = babyMeatNutPerInput(d);
				float b = adultMeatNutPerInput(d);
				return Mathf.Max(a, b);
			};
			Func<ThingDef, string> eggNut = delegate(ThingDef d)
			{
				CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				if (compProperties == null)
				{
					return string.Empty;
				}
				return compProperties.eggFertilizedDef.ingestible.nutrition.ToString("F2");
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh
			orderby bestMeatPerInput(d) descending
			select d, new TableDataGetter<ThingDef>("animal", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => d.race.baseHungerRate.ToString("F2")), new TableDataGetter<ThingDef>("gestDays", (ThingDef d) => gestDays(d).ToString("F2")), new TableDataGetter<ThingDef>("herbiv", (ThingDef d) => ((d.race.foodType & FoodTypeFlags.Plant) == FoodTypeFlags.None) ? string.Empty : "Y"), new TableDataGetter<ThingDef>("eggs", (ThingDef d) => (!d.HasComp(typeof(CompEggLayer))) ? string.Empty : d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.ToString()), new TableDataGetter<ThingDef>("|", (ThingDef d) => "|"), new TableDataGetter<ThingDef>("bodySize", (ThingDef d) => d.race.baseBodySize.ToString("F2")), new TableDataGetter<ThingDef>("age Adult", (ThingDef d) => d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge.ToString("F2")), new TableDataGetter<ThingDef>("nutrition to adulthood", (ThingDef d) => nutritionToAdulthood(d).ToString("F2")), new TableDataGetter<ThingDef>("adult meat-nut", (ThingDef d) => ((float)(d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05000000074505806)).ToString("F2")), new TableDataGetter<ThingDef>("adult meat-nut / input-nut", (ThingDef d) => adultMeatNutPerInput(d).ToString("F3")), new TableDataGetter<ThingDef>("|", (ThingDef d) => "|"), new TableDataGetter<ThingDef>("baby size", (ThingDef d) => (d.race.lifeStageAges[0].def.bodySizeFactor * d.race.baseBodySize).ToString("F2")), new TableDataGetter<ThingDef>("nutrition to gestate", (ThingDef d) => nutritionToGestate(d).ToString("F2")), new TableDataGetter<ThingDef>("egg nut", (ThingDef d) => eggNut(d)), new TableDataGetter<ThingDef>("baby meat-nut", (ThingDef d) => babyMeatNut(d).ToString("F2")), new TableDataGetter<ThingDef>("baby meat-nut / input-nut", (ThingDef d) => babyMeatNutPerInput(d).ToString("F2")), new TableDataGetter<ThingDef>("baby wins", (ThingDef d) => (!(babyMeatNutPerInput(d) > adultMeatNutPerInput(d))) ? string.Empty : "B"));
		}

		public static void DoTable_CropEconomy()
		{
			Func<ThingDef, float> workCost = delegate(ThingDef d)
			{
				float num = 1.1f;
				num = (float)(num + d.plant.growDays * 1.0);
				return (float)(num + (d.plant.sowWork + d.plant.harvestWork) * 0.006120000034570694);
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant && d.plant.Harvestable && d.plant.Sowable
			orderby d.plant.IsTree
			select d, new TableDataGetter<ThingDef>("plant", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("product", (ThingDef d) => d.plant.harvestedThingDef.defName), new TableDataGetter<ThingDef>("grow time", (ThingDef d) => d.plant.growDays.ToString("F1")), new TableDataGetter<ThingDef>("work", (ThingDef d) => (d.plant.sowWork + d.plant.harvestWork).ToString("F0")), new TableDataGetter<ThingDef>("harvestCount", (ThingDef d) => d.plant.harvestYield.ToString("F1")), new TableDataGetter<ThingDef>("work-cost per cycle", (ThingDef d) => workCost(d).ToString("F2")), new TableDataGetter<ThingDef>("work-cost per harvestCount", (ThingDef d) => (workCost(d) / d.plant.harvestYield).ToString("F2")), new TableDataGetter<ThingDef>("value each", (ThingDef d) => d.plant.harvestedThingDef.BaseMarketValue.ToString("F2")), new TableDataGetter<ThingDef>("harvestValueTotal", (ThingDef d) => (d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue).ToString("F2")), new TableDataGetter<ThingDef>("profit per growDay", (ThingDef d) => ((d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue - workCost(d)) / d.plant.growDays).ToString("F2")), new TableDataGetter<ThingDef>("nutrition per growDay", (ThingDef d) => (d.plant.harvestedThingDef.ingestible == null) ? string.Empty : (d.plant.harvestYield * d.plant.harvestedThingDef.ingestible.nutrition / d.plant.growDays).ToString("F2")), new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => (d.plant.harvestedThingDef.ingestible == null) ? string.Empty : d.plant.harvestedThingDef.ingestible.nutrition.ToString("F2")));
		}

		public static void DoTable_ItemNutritions()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.IsNutritionGivingIngestible
			orderby d.ingestible.nutrition
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.BaseMarketValue.ToString("F1")), new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => d.ingestible.nutrition.ToString("F2")), new TableDataGetter<ThingDef>("nutrition per value", (ThingDef d) => (d.ingestible.nutrition / d.BaseMarketValue).ToString("F3")), new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, null).ToString("F0")));
		}

		public static void DoTable_AllNutritions()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.ingestible != null
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => d.ingestible.nutrition.ToStringPercentEmptyZero("F0")));
		}

		public static void DoTable_ItemMarketValuesStackable()
		{
			DataAnalysisTableMaker.DoItemMarketValues(true);
		}

		public static void DoTable_ItemMarketValuesUnstackable()
		{
			DataAnalysisTableMaker.DoItemMarketValues(false);
		}

		private static void DoItemMarketValues(bool stackable)
		{
			Func<ThingDef, float> workAmountGetter = delegate(ThingDef d)
			{
				if (d.recipeMaker == null)
				{
					return -1f;
				}
				if (d.recipeMaker.workAmount >= 0)
				{
					return (float)d.recipeMaker.workAmount;
				}
				return Mathf.Max(d.GetStatValueAbstract(StatDefOf.WorkToMake, null), d.GetStatValueAbstract(StatDefOf.WorkToBuild, null));
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.BaseMarketValue > 0.0099999997764825821 && d.stackLimit > 1 == stackable
			orderby d.BaseMarketValue
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("base market value", (ThingDef d) => d.BaseMarketValue.ToString("F1")), new TableDataGetter<ThingDef>("cost to make", (ThingDef d) => DataAnalysisTableMaker.CostToMakeString(d, false)), new TableDataGetter<ThingDef>("work to make", (ThingDef d) => (d.recipeMaker == null) ? "-" : workAmountGetter(d).ToString("F1")), new TableDataGetter<ThingDef>("profit", (ThingDef d) => (d.BaseMarketValue - DataAnalysisTableMaker.CostToMake(d, false)).ToString("F1")), new TableDataGetter<ThingDef>("profit rate", (ThingDef d) => (d.recipeMaker == null) ? "-" : ((float)((d.BaseMarketValue - DataAnalysisTableMaker.CostToMake(d, false)) / workAmountGetter(d) * 10000.0)).ToString("F0")));
		}

		public static void DoTable_Stuffs()
		{
			Func<ThingDef, StatDef, string> workGetter = delegate(ThingDef d, StatDef stat)
			{
				if (d.stuffProps.statFactors == null)
				{
					return string.Empty;
				}
				StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((StatModifier fa) => fa.stat == stat);
				if (statModifier == null)
				{
					return string.Empty;
				}
				return statModifier.value.ToString();
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby d.BaseMarketValue
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("base market value", (ThingDef d) => d.BaseMarketValue.ToString("F1")), new TableDataGetter<ThingDef>("fac-WorkToMake", (ThingDef d) => workGetter(d, StatDefOf.WorkToMake)), new TableDataGetter<ThingDef>("fac-WorkToBuild", (ThingDef d) => workGetter(d, StatDefOf.WorkToBuild)));
		}

		public static void DoTable_ProductionRecipes()
		{
			Func<RecipeDef, float> trueWork = (RecipeDef d) => d.WorkAmountTotal(null);
			Func<RecipeDef, float> cheapestIngredientVal = delegate(RecipeDef d)
			{
				float num2 = 0f;
				foreach (IngredientCount ingredient in d.ingredients)
				{
					num2 += ingredient.filter.AllowedThingDefs.Min((ThingDef td) => td.BaseMarketValue) * ingredient.GetBaseCount();
				}
				return num2;
			};
			Func<RecipeDef, float> workVal = (RecipeDef d) => (float)(trueWork(d) * 0.0099999997764825821);
			Func<RecipeDef, float> cheapestProductsVal = delegate(RecipeDef d)
			{
				ThingDef thingDef = d.ingredients.First().filter.AllowedThingDefs.MinBy((ThingDef td) => td.BaseMarketValue);
				float num = 0f;
				foreach (ThingCountClass product in d.products)
				{
					num += product.thingDef.GetStatValueAbstract(StatDefOf.MarketValue, (!product.thingDef.MadeFromStuff) ? null : thingDef) * (float)product.count;
				}
				return num;
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<RecipeDef>.AllDefs
			where !d.products.NullOrEmpty() && !d.ingredients.NullOrEmpty()
			select d, new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName), new TableDataGetter<RecipeDef>("work", (RecipeDef d) => trueWork(d).ToString("F0")), new TableDataGetter<RecipeDef>("cheapest ingredients value", (RecipeDef d) => cheapestIngredientVal(d).ToString("F1")), new TableDataGetter<RecipeDef>("work value", (RecipeDef d) => workVal(d).ToString("F1")), new TableDataGetter<RecipeDef>("cheapest products value", (RecipeDef d) => cheapestProductsVal(d).ToString("F1")), new TableDataGetter<RecipeDef>("profit raw", (RecipeDef d) => (cheapestProductsVal(d) - cheapestIngredientVal(d)).ToString("F1")), new TableDataGetter<RecipeDef>("profit with work", (RecipeDef d) => (cheapestProductsVal(d) - workVal(d) - cheapestIngredientVal(d)).ToString("F1")), new TableDataGetter<RecipeDef>("profit per work day", (RecipeDef d) => ((float)((cheapestProductsVal(d) - cheapestIngredientVal(d)) * 60000.0 / trueWork(d))).ToString("F0")));
		}

		private static string CostToMakeString(ThingDef d, bool real = false)
		{
			if (d.recipeMaker == null)
			{
				return "-";
			}
			return DataAnalysisTableMaker.CostToMake(d, real).ToString("F1");
		}

		private static float CostToMake(ThingDef d, bool real = false)
		{
			if (d.recipeMaker == null)
			{
				return d.BaseMarketValue;
			}
			float num = 0f;
			if (d.costList != null)
			{
				foreach (ThingCountClass cost in d.costList)
				{
					float num2 = 1f;
					if (real)
					{
						num2 = (float)((!DataAnalysisTableMaker.RequiresBuying(cost.thingDef)) ? 0.5 : 1.5);
					}
					num += (float)cost.count * DataAnalysisTableMaker.CostToMake(cost.thingDef, true) * num2;
				}
			}
			if (d.costStuffCount > 0)
			{
				ThingDef thingDef = GenStuff.DefaultStuffFor(d);
				num += (float)d.costStuffCount * thingDef.BaseMarketValue;
			}
			return num;
		}

		private static bool RequiresBuying(ThingDef def)
		{
			if (def.costList != null)
			{
				foreach (ThingCountClass cost in def.costList)
				{
					if (DataAnalysisTableMaker.RequiresBuying(cost.thingDef))
					{
						return true;
					}
				}
				return false;
			}
			return !DefDatabase<ThingDef>.AllDefs.Any((ThingDef d) => d.plant != null && d.plant.harvestedThingDef == def && d.plant.Sowable);
		}

		public static void DoTable_RacesBasics()
		{
			Func<PawnKindDef, float> dps = (PawnKindDef d) => DataAnalysisTableMaker.RaceMeleeDpsEstimate(d.race);
			Func<PawnKindDef, float> pointsGuess = delegate(PawnKindDef d)
			{
				float num2 = 15f;
				num2 = (float)(num2 + dps(d) * 10.0);
				num2 *= Mathf.Lerp(1f, (float)(d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null) / 3.0), 0.25f);
				num2 *= d.RaceProps.baseHealthScale;
				num2 *= GenMath.LerpDouble(0.25f, 1f, 1.65f, 1f, Mathf.Clamp(d.RaceProps.baseBodySize, 0.25f, 1f));
				return (float)(num2 * 0.75999999046325684);
			};
			Func<PawnKindDef, float> mktValGuess = delegate(PawnKindDef d)
			{
				float num = 18f;
				num = (float)(num + pointsGuess(d) * 2.7000000476837158);
				if (d.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.None)
				{
					num = (float)(num * 0.5);
					goto IL_00a3;
				}
				if (d.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Simple)
				{
					num = (float)(num * 0.800000011920929);
					goto IL_00a3;
				}
				if (d.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Intermediate)
				{
					num = num;
					goto IL_00a3;
				}
				if (d.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Advanced)
				{
					num = (float)(num + 250.0);
					goto IL_00a3;
				}
				throw new InvalidOperationException();
				IL_00a3:
				num = (float)(num + d.RaceProps.baseBodySize * 80.0);
				if (d.race.HasComp(typeof(CompMilkable)))
				{
					num = (float)(num + 125.0);
				}
				if (d.race.HasComp(typeof(CompShearable)))
				{
					num = (float)(num + 90.0);
				}
				if (d.race.HasComp(typeof(CompEggLayer)))
				{
					num = (float)(num + 90.0);
				}
				num *= Mathf.Lerp(0.8f, 1.2f, d.RaceProps.wildness);
				return (float)(num * 0.75);
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && !d.RaceProps.Humanlike
			select d, new TableDataGetter<PawnKindDef>("defName", (PawnKindDef d) => d.defName), new TableDataGetter<PawnKindDef>("points", (PawnKindDef d) => d.combatPower.ToString("F0")), new TableDataGetter<PawnKindDef>("points guess", (PawnKindDef d) => pointsGuess(d).ToString("F0")), new TableDataGetter<PawnKindDef>("mktval", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0")), new TableDataGetter<PawnKindDef>("mktval guess", (PawnKindDef d) => mktValGuess(d).ToString("F0")), new TableDataGetter<PawnKindDef>("healthScale", (PawnKindDef d) => d.RaceProps.baseHealthScale.ToString("F2")), new TableDataGetter<PawnKindDef>("bodySize", (PawnKindDef d) => d.RaceProps.baseBodySize.ToString("F2")), new TableDataGetter<PawnKindDef>("hunger rate", (PawnKindDef d) => d.RaceProps.baseHungerRate.ToString("F2")), new TableDataGetter<PawnKindDef>("speed", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString("F2")), new TableDataGetter<PawnKindDef>("melee dps", (PawnKindDef d) => dps(d).ToString("F2")), new TableDataGetter<PawnKindDef>("wildness", (PawnKindDef d) => d.RaceProps.wildness.ToStringPercent()), new TableDataGetter<PawnKindDef>("life expec.", (PawnKindDef d) => d.RaceProps.lifeExpectancy.ToString("F1")), new TableDataGetter<PawnKindDef>("train-int", (PawnKindDef d) => d.RaceProps.TrainableIntelligence.label), new TableDataGetter<PawnKindDef>("temps", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null).ToString("F0") + ".." + d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null).ToString("F0")), new TableDataGetter<PawnKindDef>("mateMtb", (PawnKindDef d) => d.RaceProps.mateMtbHours.ToStringEmptyZero("F0")), new TableDataGetter<PawnKindDef>("nuzzMtb", (PawnKindDef d) => d.RaceProps.nuzzleMtbHours.ToStringEmptyZero("F0")), new TableDataGetter<PawnKindDef>("mhChDam", (PawnKindDef d) => d.RaceProps.manhunterOnDamageChance.ToStringPercentEmptyZero("F2")), new TableDataGetter<PawnKindDef>("mhChTam", (PawnKindDef d) => d.RaceProps.manhunterOnTameFailChance.ToStringPercentEmptyZero("F2")));
		}

		private static float RaceMeleeDpsEstimate(ThingDef race)
		{
			if (race.Verbs.NullOrEmpty())
			{
				return 0f;
			}
			IEnumerable<VerbProperties> list = from v in race.Verbs
			where (float)v.meleeDamageBaseAmount > 0.0010000000474974513
			select v;
			return list.AverageWeighted((VerbProperties v) => v.BaseMeleeSelectionWeight, (VerbProperties v) => (float)v.meleeDamageBaseAmount / (v.defaultCooldownTime + v.warmupTime));
		}

		public static void DoTable_RacesFoodConsumption()
		{
			Func<ThingDef, int, string> lsName = delegate(ThingDef d, int lsIndex)
			{
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					return string.Empty;
				}
				LifeStageDef def3 = d.race.lifeStageAges[lsIndex].def;
				return def3.defName;
			};
			Func<ThingDef, int, string> maxFood = delegate(ThingDef d, int lsIndex)
			{
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					return string.Empty;
				}
				LifeStageDef def2 = d.race.lifeStageAges[lsIndex].def;
				return (d.race.baseBodySize * def2.bodySizeFactor * def2.foodMaxFactor).ToString("F2");
			};
			Func<ThingDef, int, string> hungerRate = delegate(ThingDef d, int lsIndex)
			{
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					return string.Empty;
				}
				LifeStageDef def = d.race.lifeStageAges[lsIndex].def;
				return (d.race.baseHungerRate * def.hungerRateFactor).ToString("F2");
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null && d.race.EatsFood
			orderby d.race.baseHungerRate descending
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("Lifestage 0", (ThingDef d) => lsName(d, 0)), new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 0)), new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 0)), new TableDataGetter<ThingDef>("Lifestage 1", (ThingDef d) => lsName(d, 1)), new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 1)), new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 1)), new TableDataGetter<ThingDef>("Lifestage 2", (ThingDef d) => lsName(d, 2)), new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 2)), new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 2)), new TableDataGetter<ThingDef>("Lifestage 3", (ThingDef d) => lsName(d, 3)), new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 3)), new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 3)));
		}

		public static void DoTable_AnimalBiomeCommonalities()
		{
			List<TableDataGetter<PawnKindDef>> list = (from b in DefDatabase<BiomeDef>.AllDefs
			where b.implemented && b.canBuildBase
			orderby b.animalDensity
			select new TableDataGetter<PawnKindDef>(b.defName, delegate(PawnKindDef k)
			{
				float num = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki));
				float num2 = b.CommonalityOfAnimal(k);
				float num3 = num2 / num;
				if (num3 == 0.0)
				{
					return string.Empty;
				}
				return num3.ToStringPercent("F1");
			})).ToList();
			list.Insert(0, new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName + ((!k.race.race.predator) ? string.Empty : "*")));
			DebugTables.MakeTablesDialog(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.defName
			select d, list.ToArray());
		}

		public static void DoTable_AnimalCombatBalance()
		{
			Func<PawnKindDef, float> meleeDps = delegate(PawnKindDef k)
			{
				Pawn pawn2 = PawnGenerator.GeneratePawn(k, null);
				while (pawn2.health.hediffSet.hediffs.Count > 0)
				{
					pawn2.health.RemoveHediff(pawn2.health.hediffSet.hediffs[0]);
				}
				return pawn2.GetStatValue(StatDefOf.MeleeDPS, true);
			};
			Func<PawnKindDef, float> averageArmor = delegate(PawnKindDef k)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(k, null);
				while (pawn.health.hediffSet.hediffs.Count > 0)
				{
					pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
				}
				return (float)((pawn.GetStatValue(StatDefOf.ArmorRating_Blunt, true) + pawn.GetStatValue(StatDefOf.ArmorRating_Sharp, true)) / 2.0);
			};
			Func<PawnKindDef, float> combatPowerCalculated = delegate(PawnKindDef k)
			{
				float num = (float)(1.0 + meleeDps(k) * 2.0);
				float num2 = (float)(1.0 + (k.RaceProps.baseHealthScale + averageArmor(k) * 1.7999999523162842) * 2.0);
				float num3 = (float)(num * num2 * 2.5 + 10.0);
				return (float)(num3 + k.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null) * 2.0);
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.combatPower
			select d, new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName), new TableDataGetter<PawnKindDef>("meleeDps", (PawnKindDef k) => meleeDps(k).ToString("F1")), new TableDataGetter<PawnKindDef>("baseHealthScale", (PawnKindDef k) => k.RaceProps.baseHealthScale.ToString()), new TableDataGetter<PawnKindDef>("moveSpeed", (PawnKindDef k) => k.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString()), new TableDataGetter<PawnKindDef>("averageArmor", (PawnKindDef k) => averageArmor(k).ToStringPercent()), new TableDataGetter<PawnKindDef>("combatPowerCalculated", (PawnKindDef k) => combatPowerCalculated(k).ToString("F0")), new TableDataGetter<PawnKindDef>("combatPower", (PawnKindDef k) => k.combatPower.ToString()));
		}

		public static void DoTable_PlantsBasics()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.plant.fertilitySensitivity
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("growDays", (ThingDef d) => d.plant.growDays.ToString("F2")), new TableDataGetter<ThingDef>("reproduceMtb", (ThingDef d) => d.plant.reproduceMtbDays.ToString("F2")), new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => (d.ingestible == null) ? "-" : d.ingestible.nutrition.ToString("F2")), new TableDataGetter<ThingDef>("nut/day", (ThingDef d) => (d.ingestible == null) ? "-" : (d.ingestible.nutrition / d.plant.growDays).ToString("F4")), new TableDataGetter<ThingDef>("fertilityMin", (ThingDef d) => d.plant.fertilityMin.ToString("F2")), new TableDataGetter<ThingDef>("fertilitySensitivity", (ThingDef d) => d.plant.fertilitySensitivity.ToString("F2")), new TableDataGetter<ThingDef>("blightable", (ThingDef d) => (!d.plant.Blightable) ? string.Empty : "blightable"));
		}

		public static void DoTable_WeaponPairs()
		{
			PawnWeaponGenerator.MakeTableWeaponPairs();
		}

		public static void DoTable_WeaponPairsByThing()
		{
			PawnWeaponGenerator.MakeTableWeaponPairsByThing();
		}

		public static void DoTable_WeaponsRanged()
		{
			Func<ThingDef, int> damage = (ThingDef d) => (d.Verbs[0].defaultProjectile != null) ? d.Verbs[0].defaultProjectile.projectile.damageAmountBase : 0;
			Func<ThingDef, float> warmup = (ThingDef d) => d.Verbs[0].warmupTime;
			Func<ThingDef, float> cooldown = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, null);
			Func<ThingDef, int> burstShots = (ThingDef d) => d.Verbs[0].burstShotCount;
			Func<ThingDef, float> dpsMissless = delegate(ThingDef d)
			{
				int num2 = burstShots(d);
				float num3 = warmup(d) + cooldown(d);
				num3 = (float)(num3 + (float)(num2 - 1) * ((float)d.Verbs[0].ticksBetweenBurstShots / 60.0));
				return (float)(damage(d) * num2) / num3;
			};
			Func<ThingDef, float> accTouch = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyTouch, null);
			Func<ThingDef, float> accShort = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyShort, null);
			Func<ThingDef, float> accMed = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyMedium, null);
			Func<ThingDef, float> accLong = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyLong, null);
			Func<ThingDef, float> dpsAvg = delegate(ThingDef d)
			{
				float num = 0f;
				num += dpsMissless(d) * accShort(d);
				num += dpsMissless(d) * accMed(d);
				num += dpsMissless(d) * accMed(d);
				num += dpsMissless(d) * accLong(d);
				return (float)(num / 4.0);
			};
			DebugTables.MakeTablesDialog((from d in DefDatabase<ThingDef>.AllDefs
			where d.IsRangedWeapon
			select d).OrderByDescending(dpsAvg), new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("damage", (ThingDef d) => damage(d).ToString()), new TableDataGetter<ThingDef>("warmup", (ThingDef d) => warmup(d).ToString("F2")), new TableDataGetter<ThingDef>("burst", (ThingDef d) => burstShots(d).ToString()), new TableDataGetter<ThingDef>("cooldown", (ThingDef d) => cooldown(d).ToString("F2")), new TableDataGetter<ThingDef>("range", (ThingDef d) => d.Verbs[0].range.ToString("F0")), new TableDataGetter<ThingDef>("dpsMissless", (ThingDef d) => dpsMissless(d).ToString("F2")), new TableDataGetter<ThingDef>("accTouch", (ThingDef d) => accTouch(d).ToStringPercent()), new TableDataGetter<ThingDef>("accShort", (ThingDef d) => accShort(d).ToStringPercent()), new TableDataGetter<ThingDef>("accMed", (ThingDef d) => accMed(d).ToStringPercent()), new TableDataGetter<ThingDef>("accLong", (ThingDef d) => accLong(d).ToStringPercent()), new TableDataGetter<ThingDef>("dpsTouch", (ThingDef d) => (dpsMissless(d) * accTouch(d)).ToString("F2")), new TableDataGetter<ThingDef>("dpsShort", (ThingDef d) => (dpsMissless(d) * accShort(d)).ToString("F2")), new TableDataGetter<ThingDef>("dpsMed", (ThingDef d) => (dpsMissless(d) * accMed(d)).ToString("F2")), new TableDataGetter<ThingDef>("dpsLong", (ThingDef d) => (dpsMissless(d) * accLong(d)).ToString("F2")), new TableDataGetter<ThingDef>("dpsAvg", (ThingDef d) => dpsAvg(d).ToString("F2")), new TableDataGetter<ThingDef>("mktVal", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0")), new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, null).ToString("F0")), new TableDataGetter<ThingDef>("mktVal/dpsAvg", (ThingDef d) => (d.GetStatValueAbstract(StatDefOf.MarketValue, null) / dpsAvg(d)).ToString("F2")));
		}

		public static void DoTable_WeaponsMeleeStuffless()
		{
			DataAnalysisTableMaker.DoTablesInternal_Melee(null, false);
		}

		public static void DoTable_WeaponsMeleeSteel()
		{
			DataAnalysisTableMaker.DoTablesInternal_Melee(ThingDefOf.Steel, false);
		}

		public static void DoTable_WeaponsMeleeWood()
		{
			DataAnalysisTableMaker.DoTablesInternal_Melee(ThingDefOf.WoodLog, false);
		}

		public static void DoTable_WeaponsMeleePlasteel()
		{
			DataAnalysisTableMaker.DoTablesInternal_Melee(ThingDefOf.Plasteel, false);
		}

		public static void DoTable_MeleeSteelAndRaces()
		{
			DataAnalysisTableMaker.DoTablesInternal_Melee(ThingDefOf.Steel, true);
		}

		private static void DoTablesInternal_Melee(ThingDef stuff, bool doRaces = false)
		{
			Func<Def, float> meleeDamageGetter = delegate(Def d)
			{
				Thing owner2;
				List<Verb> concreteExampleVerbs5 = VerbUtility.GetConcreteExampleVerbs(d, out owner2, stuff);
				if (concreteExampleVerbs5.OfType<Verb_MeleeAttack>().Any())
				{
					return concreteExampleVerbs5.OfType<Verb_MeleeAttack>().AverageWeighted((Verb_MeleeAttack v) => v.verbProps.AdjustedMeleeSelectionWeight(v, null, owner2), (Verb_MeleeAttack v) => v.verbProps.AdjustedMeleeDamageAmount(v, null, owner2));
				}
				return -1f;
			};
			Func<Def, float> rangedDamageGetter = delegate(Def d)
			{
				Thing thing3 = default(Thing);
				List<Verb> concreteExampleVerbs4 = VerbUtility.GetConcreteExampleVerbs(d, out thing3, stuff);
				Verb verb3 = concreteExampleVerbs4.OfType<Verb_LaunchProjectile>().FirstOrDefault();
				if (verb3 != null && verb3.GetProjectile() != null)
				{
					return (float)verb3.GetProjectile().projectile.damageAmountBase;
				}
				return -1f;
			};
			Func<Def, float> meleeWarmupGetter = (Def d) => 0f;
			Func<Def, float> rangedWarmupGetter = delegate(Def d)
			{
				Thing thing2 = default(Thing);
				List<Verb> concreteExampleVerbs3 = VerbUtility.GetConcreteExampleVerbs(d, out thing2, stuff);
				Verb verb2 = concreteExampleVerbs3.OfType<Verb_LaunchProjectile>().FirstOrDefault();
				if (verb2 != null)
				{
					return verb2.verbProps.warmupTime;
				}
				return -1f;
			};
			Func<Def, float> meleeCooldownGetter = delegate(Def d)
			{
				Thing owner;
				List<Verb> concreteExampleVerbs2 = VerbUtility.GetConcreteExampleVerbs(d, out owner, stuff);
				if (concreteExampleVerbs2.OfType<Verb_MeleeAttack>().Any())
				{
					return concreteExampleVerbs2.OfType<Verb_MeleeAttack>().AverageWeighted((Verb_MeleeAttack v) => v.verbProps.AdjustedMeleeSelectionWeight(v, null, owner), (Verb_MeleeAttack v) => v.verbProps.AdjustedCooldown(v, null, owner));
				}
				return -1f;
			};
			Func<Def, float> rangedCooldownGetter = delegate(Def d)
			{
				Thing thing = default(Thing);
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out thing, stuff);
				Verb verb = concreteExampleVerbs.OfType<Verb_LaunchProjectile>().FirstOrDefault();
				if (verb != null)
				{
					return verb.verbProps.defaultCooldownTime;
				}
				return -1f;
			};
			Func<Def, float> meleeDpsGetter = (Def d) => meleeDamageGetter(d) / (meleeWarmupGetter(d) + meleeCooldownGetter(d));
			Func<Def, float> rangedDpsGetter = (Def d) => rangedDamageGetter(d) / (rangedWarmupGetter(d) + rangedCooldownGetter(d));
			Func<Def, float> dpsGetter = (Def d) => Mathf.Max(meleeDpsGetter(d), rangedDpsGetter(d));
			Func<Def, float> marketValueGetter = delegate(Def d)
			{
				ThingDef thingDef2 = d as ThingDef;
				if (thingDef2 != null)
				{
					return thingDef2.GetStatValueAbstract(StatDefOf.MarketValue, stuff);
				}
				HediffDef hediffDef = d as HediffDef;
				if (hediffDef != null)
				{
					if (hediffDef.spawnThingOnRemoved == null)
					{
						return 0f;
					}
					return hediffDef.spawnThingOnRemoved.GetStatValueAbstract(StatDefOf.MarketValue, null);
				}
				return -1f;
			};
			IEnumerable<Def> enumerable = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsMeleeWeapon
			select d).Cast<Def>().Concat((from h in DefDatabase<HediffDef>.AllDefs
			where h.CompProps<HediffCompProperties_VerbGiver>() != null
			select h).Cast<Def>());
			if (doRaces)
			{
				enumerable = enumerable.Concat((from d in DefDatabase<ThingDef>.AllDefs
				where d.race != null
				select d).Cast<Def>());
			}
			enumerable = from h in enumerable
			orderby dpsGetter(h) descending
			select h;
			DebugTables.MakeTablesDialog(enumerable, new TableDataGetter<Def>("defName", (Def d) => d.defName), new TableDataGetter<Def>("mDamage", (Def d) => meleeDamageGetter(d).ToString()), new TableDataGetter<Def>("mWarmup", (Def d) => meleeWarmupGetter(d).ToString("F2")), new TableDataGetter<Def>("mCooldown", (Def d) => meleeCooldownGetter(d).ToString("F2")), new TableDataGetter<Def>("mDps", (Def d) => meleeDpsGetter(d).ToString("F2")), new TableDataGetter<Def>("rDamage", (Def d) => rangedDamageGetter(d).ToString()), new TableDataGetter<Def>("rWarmup", (Def d) => rangedWarmupGetter(d).ToString("F2")), new TableDataGetter<Def>("rCooldown", (Def d) => rangedCooldownGetter(d).ToString("F2")), new TableDataGetter<Def>("rDps", (Def d) => rangedDpsGetter(d).ToString("F2")), new TableDataGetter<Def>("dps", (Def d) => dpsGetter(d).ToString("F2")), new TableDataGetter<Def>("mktval", (Def d) => marketValueGetter(d).ToString("F0")), new TableDataGetter<Def>("work", delegate(Def d)
			{
				ThingDef thingDef = d as ThingDef;
				if (thingDef == null)
				{
					return "-";
				}
				return thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0");
			}));
		}

		public static void DoTable_WeaponUsage()
		{
			List<TableDataGetter<PawnKindDef>> list = new List<TableDataGetter<PawnKindDef>>();
			list.Add(new TableDataGetter<PawnKindDef>("defName", (PawnKindDef x) => x.defName));
			list.AddRange(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsWeapon && !x.weaponTags.NullOrEmpty() && x.canBeSpawningInventory
			orderby x.IsMeleeWeapon descending, x.techLevel, x.BaseMarketValue
			select new TableDataGetter<PawnKindDef>(GenText.WithoutVowelsIfLong(x.label), delegate(PawnKindDef y)
			{
				if (x.weaponTags.Any((string z) => y.weaponTags.Contains(z)))
				{
					if (y.weaponMoney.max < PawnWeaponGenerator.CheapestNonDerpPriceFor(x))
					{
						return "  no $";
					}
					return "   ✓";
				}
				return string.Empty;
			}));
			list.Add(new TableDataGetter<PawnKindDef>("avg $", (PawnKindDef x) => x.weaponMoney.Average.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("min $", (PawnKindDef x) => x.weaponMoney.min.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("max $", (PawnKindDef x) => x.weaponMoney.max.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("points", (PawnKindDef x) => x.combatPower.ToString()));
			DebugTables.MakeTablesDialog(from x in DefDatabase<PawnKindDef>.AllDefs
			where !x.weaponTags.NullOrEmpty()
			orderby (x.defaultFactionType == null) ? 2147483647 : ((int)x.defaultFactionType.techLevel), x.combatPower
			select x, list.ToArray());
		}

		public static void DoTable_ApparelPairs()
		{
			PawnApparelGenerator.MakeTableApparelPairs();
		}

		public static void DoTable_ApparelPairsByThing()
		{
			PawnApparelGenerator.MakeTableApparelPairsByThing();
		}

		public static void DoTable_ApparelPairsHeadwearLog()
		{
			PawnApparelGenerator.LogHeadwearApparelPairs();
		}

		public static void DoTable_ApparelSpawnStats()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef item in from pk in DefDatabase<PawnKindDef>.AllDefs
			where pk.race.race.Humanlike
			select pk)
			{
				PawnKindDef kind = item;
				list.Add(new FloatMenuOption(kind.defName, delegate
				{
					Faction faction = FactionUtility.DefaultFactionFrom(kind.defaultFactionType);
					DefMap<ThingDef, int> appCounts = new DefMap<ThingDef, int>();
					for (int i = 0; i < 200; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(kind, faction);
						foreach (Apparel item2 in pawn.apparel.WornApparel)
						{
							DefMap<ThingDef, int> defMap;
							ThingDef def;
							(defMap = appCounts)[def = item2.def] = defMap[def] + 1;
						}
					}
					DebugTables.MakeTablesDialog(DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.IsApparel && appCounts[d] > 0).OrderByDescending((ThingDef d) => appCounts[d]), new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("count of " + 200, (ThingDef d) => appCounts[d].ToString()), new TableDataGetter<ThingDef>("percent of pawns", (ThingDef d) => ((float)((float)appCounts[d] / 200.0)).ToStringPercent("F2")));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static void DoTable_ApparelStuffless()
		{
			DataAnalysisTableMaker.DoTablesInternal_Apparel(null);
		}

		public static void DoTable_ApparelCloth()
		{
			DataAnalysisTableMaker.DoTablesInternal_Apparel(ThingDefOf.Cloth);
		}

		public static void DoTable_ApparelHyperweave()
		{
			DataAnalysisTableMaker.DoTablesInternal_Apparel(ThingDefOf.Hyperweave);
		}

		public static void DoTable_ApparelHumanleather()
		{
			DataAnalysisTableMaker.DoTablesInternal_Apparel(ThingDef.Named("Human_Leather"));
		}

		private static void DoTablesInternal_Apparel(ThingDef stuff)
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.IsApparel
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("bodyParts", (ThingDef d) => GenText.ToSpaceList(d.apparel.bodyPartGroups.Select((BodyPartGroupDef bp) => bp.defName))), new TableDataGetter<ThingDef>("layers", (ThingDef d) => GenText.ToSpaceList(d.apparel.layers.Select((ApparelLayer l) => l.ToString()))), new TableDataGetter<ThingDef>("tags", (ThingDef d) => GenText.ToSpaceList(d.apparel.tags.Select((string t) => t.ToString()))), new TableDataGetter<ThingDef>("insCold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff).ToString("F0")), new TableDataGetter<ThingDef>("insHeat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff).ToString("F0")), new TableDataGetter<ThingDef>("blunt", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuff).ToString("F2")), new TableDataGetter<ThingDef>("sharp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuff).ToString("F2")), new TableDataGetter<ThingDef>("mktval", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, stuff).ToString("F0")), new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0")));
		}

		public static void DoTable_HitPoints()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.useHitPoints
			orderby d.GetStatValueAbstract(StatDefOf.MaxHitPoints, null) descending
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("hp", (ThingDef d) => d.BaseMaxHitPoints.ToString()), new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString()));
		}

		public static void DoTable_FillPercent()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.fillPercent > 0.0
			orderby d.fillPercent descending
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("fillPercent", (ThingDef d) => d.fillPercent.ToStringPercent()), new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString()));
		}

		public static void DoTable_DeteriorationRates()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) > 0.0
			orderby d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) descending
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("deterioration rate", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null).ToString("F1")), new TableDataGetter<ThingDef>("hp", (ThingDef d) => d.BaseMaxHitPoints.ToString()), new TableDataGetter<ThingDef>("days to vanish", (ThingDef d) => ((float)d.BaseMaxHitPoints / d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null)).ToString("0.#")));
		}

		public static void DoTable_ShootingAccuracy()
		{
			StatDef stat = StatDefOf.ShootingAccuracy;
			Func<int, float, int, float> accAtDistance = delegate(int level, float dist, int traitDegree)
			{
				float num = 1f;
				if (traitDegree != 0)
				{
					float value = TraitDef.Named("ShootingAccuracy").DataAtDegree(traitDegree).statOffsets.First((StatModifier so) => so.stat == stat).value;
					num += value;
				}
				foreach (SkillNeed skillNeedFactor in stat.skillNeedFactors)
				{
					SkillNeed_Direct skillNeed_Direct = skillNeedFactor as SkillNeed_Direct;
					num *= skillNeed_Direct.valuesPerLevel[level];
				}
				num = stat.postProcessCurve.Evaluate(num);
				return Mathf.Pow(num, dist);
			};
			List<int> list = new List<int>();
			for (int i = 0; i <= 20; i++)
			{
				list.Add(i);
			}
			DebugTables.MakeTablesDialog(list, new TableDataGetter<int>("No trait skill", (int lev) => lev.ToString()), new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 0).ToStringPercent("F2")), new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 0).ToStringPercent("F2")), new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 0).ToStringPercent("F2")), new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 0).ToStringPercent("F2")), new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 0).ToStringPercent("F2")), new TableDataGetter<int>("Careful shooter skill", (int lev) => lev.ToString()), new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 1).ToStringPercent("F2")), new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 1).ToStringPercent("F2")), new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 1).ToStringPercent("F2")), new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 1).ToStringPercent("F2")), new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 1).ToStringPercent("F2")), new TableDataGetter<int>("Trigger-happy skill", (int lev) => lev.ToString()), new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, -1).ToStringPercent("F2")), new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, -1).ToStringPercent("F2")), new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, -1).ToStringPercent("F2")), new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, -1).ToStringPercent("F2")), new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, -1).ToStringPercent("F2")));
		}

		public static void DoTable_MiscIncidentChances()
		{
			List<StorytellerComp> storytellerComps = Find.Storyteller.storytellerComps;
			for (int i = 0; i < storytellerComps.Count; i++)
			{
				StorytellerComp_CategoryMTB storytellerComp_CategoryMTB = storytellerComps[i] as StorytellerComp_CategoryMTB;
				if (storytellerComp_CategoryMTB != null && ((StorytellerCompProperties_CategoryMTB)storytellerComp_CategoryMTB.props).category == IncidentCategory.Misc)
				{
					storytellerComp_CategoryMTB.DebugTablesIncidentChances(IncidentCategory.Misc);
				}
			}
		}

		public static void DoTable_BodyParts()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (BodyDef allDef in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = allDef;
				list.Add(new FloatMenuOption(localBd.defName, delegate
				{
					DebugTables.MakeTablesDialog(from d in localBd.AllParts
					orderby d.height descending
					select d, new TableDataGetter<BodyPartRecord>("defName", (BodyPartRecord d) => d.def.defName), new TableDataGetter<BodyPartRecord>("coverage", (BodyPartRecord d) => d.coverage.ToStringPercent()), new TableDataGetter<BodyPartRecord>("coverageAbsWithChildren", (BodyPartRecord d) => d.coverageAbsWithChildren.ToStringPercent()), new TableDataGetter<BodyPartRecord>("coverageAbs", (BodyPartRecord d) => d.coverageAbs.ToStringPercent()), new TableDataGetter<BodyPartRecord>("depth", (BodyPartRecord d) => d.depth.ToString()), new TableDataGetter<BodyPartRecord>("height", (BodyPartRecord d) => d.height.ToString()));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		private static void DoTable_FillPercents(ThingCategory cat)
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == cat && !d.IsFrame && d.passability != Traversability.Impassable
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("fillPercent", (ThingDef d) => d.fillPercent.ToStringPercent()));
		}

		public static void DoTable_ThingMasses()
		{
			IOrderedEnumerable<ThingDef> dataSources = from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Item || x.Minifiable
			where x.thingClass != typeof(MinifiedThing) && x.thingClass != typeof(UnfinishedThing)
			orderby x.GetStatValueAbstract(StatDefOf.Mass, null), x.GetStatValueAbstract(StatDefOf.MarketValue, null)
			select x;
			Func<ThingDef, float, string> perPawn = (ThingDef d, float bodySize) => ((float)(bodySize * 35.0 / d.GetStatValueAbstract(StatDefOf.Mass, null))).ToString("F0");
			Func<ThingDef, string> perNutrition = delegate(ThingDef d)
			{
				if (d.ingestible != null && d.ingestible.nutrition != 0.0)
				{
					return (d.GetStatValueAbstract(StatDefOf.Mass, null) / d.ingestible.nutrition).ToString("F2");
				}
				return string.Empty;
			};
			DebugTables.MakeTablesDialog(dataSources, new TableDataGetter<ThingDef>("defName", delegate(ThingDef d)
			{
				if (d.Minifiable)
				{
					return d.defName + " (minified)";
				}
				string text = d.defName;
				if (!d.EverHaulable)
				{
					text += " (not haulable)";
				}
				return text;
			}), new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Mass, null).ToString()), new TableDataGetter<ThingDef>("per human", (ThingDef d) => perPawn(d, ThingDefOf.Human.race.baseBodySize)), new TableDataGetter<ThingDef>("per muffalo", (ThingDef d) => perPawn(d, ThingDefOf.Muffalo.race.baseBodySize)), new TableDataGetter<ThingDef>("per dromedary", (ThingDef d) => perPawn(d, ThingDefOf.Dromedary.race.baseBodySize)), new TableDataGetter<ThingDef>("per nutrition", (ThingDef d) => perNutrition(d)), new TableDataGetter<ThingDef>("small volume", (ThingDef d) => (!d.smallVolume) ? string.Empty : "small"));
		}

		public static void DoTable_MedicalPotencyPerMedicine()
		{
			List<float> list = new List<float>();
			list.Add(0.3f);
			list.AddRange(from d in DefDatabase<ThingDef>.AllDefs
			where typeof(Medicine).IsAssignableFrom(d.thingClass)
			select d.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			SkillNeed_Direct skillNeed_Direct = (SkillNeed_Direct)StatDefOf.MedicalTendQuality.skillNeedFactors[0];
			TableDataGetter<float>[] array = new TableDataGetter<float>[21]
			{
				new TableDataGetter<float>("potency", GenText.ToStringPercent),
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			};
			for (int i = 0; i < 20; i++)
			{
				float factor = skillNeed_Direct.valuesPerLevel[i];
				array[i + 1] = new TableDataGetter<float>((i + 1).ToString(), (float p) => (p * factor).ToStringPercent());
			}
			DebugTables.MakeTablesDialog(list, array);
		}

		public static void DoTable_BuildingFillpercents()
		{
			DataAnalysisTableMaker.DoTable_FillPercents(ThingCategory.Building);
		}

		public static void DoTable_ItemFillpercents()
		{
			DataAnalysisTableMaker.DoTable_FillPercents(ThingCategory.Item);
		}

		public static void DoTable_TraderKinds()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			foreach (TraderKindDef allDef in DefDatabase<TraderKindDef>.AllDefs)
			{
				TraderKindDef localTk = allDef;
				string defName = localTk.defName;
				defName = defName.Replace("Caravan", "C");
				defName = defName.Replace("Visitor", "V");
				defName = defName.Replace("Orbital", "R");
				defName = defName.Replace("Neolithic", "N");
				defName = defName.Replace("Outlander", "O");
				defName = GenText.WithoutVowels(defName);
				list.Add(new TableDataGetter<ThingDef>(defName, (ThingDef td) => (!localTk.WillTrade(td)) ? string.Empty : "✓"));
			}
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.0010000000474974513 && !d.isUnfinishedThing && !d.IsCorpse && !d.destroyOnDrop && d != ThingDefOf.Silver && !d.thingCategories.NullOrEmpty()) || (d.category == ThingCategory.Building && d.Minifiable)
			orderby d.thingCategories.NullOrEmpty() ? "zzzzzzz" : d.thingCategories[0].defName, d.BaseMarketValue
			select d, list.ToArray());
		}

		public static void DoTable_Surgeries()
		{
			Func<RecipeDef, float> trueWork = (RecipeDef d) => d.WorkAmountTotal(null);
			DebugTables.MakeTablesDialog((from d in DefDatabase<RecipeDef>.AllDefs
			where d.IsSurgery
			select d).OrderByDescending(trueWork), new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName), new TableDataGetter<RecipeDef>("work", (RecipeDef d) => trueWork(d).ToString("F0")), new TableDataGetter<RecipeDef>("ingredients", (RecipeDef d) => GenText.ToCommaList(from ing in d.ingredients
			select ing.ToString(), false)), new TableDataGetter<RecipeDef>("skillRequirements", (RecipeDef d) => (d.skillRequirements != null) ? GenText.ToCommaList(from ing in d.skillRequirements
			select ing.ToString(), false) : "-"), new TableDataGetter<RecipeDef>("surgerySuccessChanceFactor", (RecipeDef d) => d.surgerySuccessChanceFactor.ToStringPercent()), new TableDataGetter<RecipeDef>("deathOnFailChance", (RecipeDef d) => d.deathOnFailedSurgeryChance.ToStringPercent()));
		}

		public static void DoTable_Terrains()
		{
			DebugTables.MakeTablesDialog(DefDatabase<TerrainDef>.AllDefs, new TableDataGetter<TerrainDef>("defName", (TerrainDef d) => d.defName), new TableDataGetter<TerrainDef>("beauty", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString()), new TableDataGetter<TerrainDef>("cleanliness", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Cleanliness, null).ToString()), new TableDataGetter<TerrainDef>("pathCost", (TerrainDef d) => d.pathCost.ToString()));
		}

		public static void DoTable_MentalBreaksMinor()
		{
			DataAnalysisTableMaker.DoMentalBreaksTable(from x in DefDatabase<MentalBreakDef>.AllDefs
			where x.intensity == MentalBreakIntensity.Minor
			select x);
		}

		public static void DoTable_MentalBreaksMajor()
		{
			DataAnalysisTableMaker.DoMentalBreaksTable(from x in DefDatabase<MentalBreakDef>.AllDefs
			where x.intensity == MentalBreakIntensity.Major
			select x);
		}

		public static void DoTable_MentalBreaksExtreme()
		{
			DataAnalysisTableMaker.DoMentalBreaksTable(from x in DefDatabase<MentalBreakDef>.AllDefs
			where x.intensity == MentalBreakIntensity.Extreme
			select x);
		}

		public static void DoTable_BestThingRequestGroup()
		{
			DebugTables.MakeTablesDialog(from x in DefDatabase<ThingDef>.AllDefs
			where ListerThings.EverListable(x, ListerThingsUse.Global) || ListerThings.EverListable(x, ListerThingsUse.Region)
			orderby x.label
			select x, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("best local", delegate(ThingDef d)
			{
				IEnumerable<ThingRequestGroup> source2 = ListerThings.EverListable(d, ListerThingsUse.Region) ? ((ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))).Where((ThingRequestGroup x) => x.StoreInRegion() && x.Includes(d)) : Enumerable.Empty<ThingRequestGroup>();
				if (!source2.Any())
				{
					return "-";
				}
				ThingRequestGroup best2 = source2.MinBy((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Region) && x.Includes(y)));
				return best2 + " (defs: " + DefDatabase<ThingDef>.AllDefs.Count((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Region) && best2.Includes(x)) + ")";
			}), new TableDataGetter<ThingDef>("best global", delegate(ThingDef d)
			{
				IEnumerable<ThingRequestGroup> source = ListerThings.EverListable(d, ListerThingsUse.Global) ? ((ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))).Where((ThingRequestGroup x) => x.Includes(d)) : Enumerable.Empty<ThingRequestGroup>();
				if (!source.Any())
				{
					return "-";
				}
				ThingRequestGroup best = source.MinBy((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Global) && x.Includes(y)));
				return best + " (defs: " + DefDatabase<ThingDef>.AllDefs.Count((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Global) && best.Includes(x)) + ")";
			}));
		}

		public static void DoTable_Drugs()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.IsDrug
			select d, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("pleasue", (ThingDef d) => (!d.IsPleasureDrug) ? string.Empty : "pleasure"), new TableDataGetter<ThingDef>("non-medical", (ThingDef d) => (!d.IsNonMedicalDrug) ? string.Empty : "non-medical"));
		}

		public static void DoTable_PawnGroupsMadeRepeated()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction allFaction in Find.FactionManager.AllFactions)
			{
				if (allFaction.def.pawnGroupMakers != null && allFaction.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Normal))
				{
					Faction localFac = allFaction;
					float localP;
					list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (float item in Dialog_DebugActionsMenu.PointsOptions())
						{
							float num = item;
							localP = num;
							list2.Add(new DebugMenuOption(localP.ToString(), DebugMenuOptionMode.Action, delegate
							{
								Dictionary<ThingDef, int>[] weaponsCount = new Dictionary<ThingDef, int>[20];
								string[] pawnKinds = new string[20];
								for (int i = 0; i < 20; i++)
								{
									weaponsCount[i] = new Dictionary<ThingDef, int>();
									PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
									pawnGroupMakerParms.tile = Find.VisibleMap.Tile;
									pawnGroupMakerParms.points = localP;
									pawnGroupMakerParms.faction = localFac;
									List<Pawn> list3 = PawnGroupMakerUtility.GeneratePawns(PawnGroupKindDefOf.Normal, pawnGroupMakerParms, false).ToList();
									pawnKinds[i] = PawnUtility.PawnKindsToCommaList(list3);
									foreach (Pawn item2 in list3)
									{
										if (item2.equipment.Primary != null)
										{
											if (!weaponsCount[i].ContainsKey(item2.equipment.Primary.def))
											{
												weaponsCount[i].Add(item2.equipment.Primary.def, 0);
											}
											Dictionary<ThingDef, int> dictionary;
											ThingDef def;
											(dictionary = weaponsCount[i])[def = item2.equipment.Primary.def] = dictionary[def] + 1;
										}
										item2.Destroy(DestroyMode.Vanish);
									}
								}
								int totalPawns = weaponsCount.Sum((Dictionary<ThingDef, int> x) => x.Sum((KeyValuePair<ThingDef, int> y) => y.Value));
								List<TableDataGetter<int>> list4 = new List<TableDataGetter<int>>();
								list4.Add(new TableDataGetter<int>(string.Empty, (int x) => (x != 20) ? (x + 1).ToString() : "avg"));
								list4.Add(new TableDataGetter<int>("pawns", (int x) => " " + ((x != 20) ? weaponsCount[x].Sum((KeyValuePair<ThingDef, int> y) => y.Value).ToString() : ((float)((float)totalPawns / 20.0)).ToString("0.#"))));
								list4.AddRange(from x in DefDatabase<ThingDef>.AllDefs
								where x.IsWeapon && !x.weaponTags.NullOrEmpty() && x.canBeSpawningInventory
								orderby x.IsMeleeWeapon descending, x.techLevel, x.BaseMarketValue
								select new TableDataGetter<int>(GenText.WithoutVowelsIfLong(x.label), delegate(int y)
								{
									if (y == 20)
									{
										return " " + ((float)((float)weaponsCount.Sum((Dictionary<ThingDef, int> z) => z.ContainsKey(x) ? z[x] : 0) / 20.0)).ToString("0.#");
									}
									return (!weaponsCount[y].ContainsKey(x)) ? string.Empty : (" " + weaponsCount[y][x] + " (" + ((float)weaponsCount[y][x] / (float)weaponsCount[y].Sum((KeyValuePair<ThingDef, int> z) => z.Value)).ToStringPercent("F0") + ")");
								}));
								list4.Add(new TableDataGetter<int>("kinds", (int x) => (x != 20) ? pawnKinds[x] : string.Empty));
								DebugTables.MakeTablesDialog(Enumerable.Range(0, 21), list4.ToArray());
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		public static void DoTable_ItemAccessibility()
		{
			DebugTables.MakeTablesDialog(from x in ItemCollectionGeneratorUtility.allGeneratableItems
			orderby x.defName
			select x, new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName), new TableDataGetter<ThingDef>("1", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1, Find.VisibleMap)) ? string.Empty : "✓"), new TableDataGetter<ThingDef>("10", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10, Find.VisibleMap)) ? string.Empty : "✓"), new TableDataGetter<ThingDef>("100", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 100, Find.VisibleMap)) ? string.Empty : "✓"), new TableDataGetter<ThingDef>("1000", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1000, Find.VisibleMap)) ? string.Empty : "✓"), new TableDataGetter<ThingDef>("10000", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10000, Find.VisibleMap)) ? string.Empty : "✓"));
		}

		private static string ToStringEmptyZero(this float f, string format)
		{
			if (f <= 0.0)
			{
				return string.Empty;
			}
			return f.ToString(format);
		}

		private static string ToStringPercentEmptyZero(this float f, string format = "F0")
		{
			if (f <= 0.0)
			{
				return string.Empty;
			}
			return f.ToStringPercent(format);
		}

		private static void DoMentalBreaksTable(IEnumerable<MentalBreakDef> breaks)
		{
			float sumWeights = breaks.Sum((MentalBreakDef x) => x.baseCommonality);
			DebugTables.MakeTablesDialog(breaks, new TableDataGetter<MentalBreakDef>("defName", (MentalBreakDef d) => d.defName), new TableDataGetter<MentalBreakDef>("chance", (MentalBreakDef d) => (d.baseCommonality / sumWeights).ToStringPercent()), new TableDataGetter<MentalBreakDef>("min duration (days)", (MentalBreakDef d) => (d.mentalState != null) ? ((float)((float)d.mentalState.minTicksBeforeRecovery / 60000.0)).ToString("0.##") : string.Empty), new TableDataGetter<MentalBreakDef>("avg duration (days)", (MentalBreakDef d) => (d.mentalState != null) ? ((float)(Mathf.Min((float)((float)d.mentalState.minTicksBeforeRecovery + d.mentalState.recoveryMtbDays * 60000.0), (float)d.mentalState.maxTicksBeforeRecovery) / 60000.0)).ToString("0.##") : string.Empty), new TableDataGetter<MentalBreakDef>("max duration (days)", (MentalBreakDef d) => (d.mentalState != null) ? ((float)((float)d.mentalState.maxTicksBeforeRecovery / 60000.0)).ToString("0.##") : string.Empty), new TableDataGetter<MentalBreakDef>("recoverFromSleep", (MentalBreakDef d) => (d.mentalState == null || !d.mentalState.recoverFromSleep) ? string.Empty : "recoverFromSleep"), new TableDataGetter<MentalBreakDef>("recoveryThought", (MentalBreakDef d) => (d.mentalState != null) ? d.mentalState.moodRecoveryThought.ToStringSafe() : string.Empty), new TableDataGetter<MentalBreakDef>("aggro", (MentalBreakDef d) => (d.mentalState == null || !d.mentalState.IsAggro) ? string.Empty : "aggro"), new TableDataGetter<MentalBreakDef>("blockNormalThoughts", (MentalBreakDef d) => (d.mentalState == null || !d.mentalState.blockNormalThoughts) ? string.Empty : "blockNormalThoughts"), new TableDataGetter<MentalBreakDef>("allowBeatfire", (MentalBreakDef d) => (d.mentalState == null || !d.mentalState.allowBeatfire) ? string.Empty : "allowBeatfire"));
		}
	}
}
