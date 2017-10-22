using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	internal static class DataAnalysisTableMaker
	{
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
			Func<ThingDef, string> ingredients = (Func<ThingDef, string>)delegate(ThingDef d)
			{
				string result;
				if (d.costList == null)
				{
					result = "-";
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (ThingCountClass cost in d.costList)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(", ");
						}
						string text = (!DataAnalysisTableMaker.RequiresBuying(cost.thingDef)) ? "" : "*";
						stringBuilder.Append(cost.thingDef.defName + text + " x" + cost.count);
					}
					result = stringBuilder.ToString().TrimEndNewlines();
				}
				return result;
			};
			Func<ThingDef, float> workAmount = (Func<ThingDef, float>)((ThingDef d) => (float)((d.recipeMaker != null) ? ((d.recipeMaker.workAmount < 0) ? Mathf.Max(d.GetStatValueAbstract(StatDefOf.WorkToMake, null), d.GetStatValueAbstract(StatDefOf.WorkToBuild, null)) : ((float)d.recipeMaker.workAmount)) : -1.0));
			Func<ThingDef, float> realIngredientCost = (Func<ThingDef, float>)((ThingDef d) => DataAnalysisTableMaker.CostToMake(d, true));
			Func<ThingDef, float> realSellPrice = (Func<ThingDef, float>)((ThingDef d) => (float)(d.BaseMarketValue * 0.5));
			Func<ThingDef, float> realBuyPrice = (Func<ThingDef, float>)((ThingDef d) => (float)(d.BaseMarketValue * 1.5));
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.IsWithinCategory(ThingCategoryDefOf.Medicine) || d.IsWithinCategory(ThingCategoryDefOf.Drugs)
			select d, new TableDataGetter<ThingDef>("name", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("ingredients", (Func<ThingDef, string>)((ThingDef d) => ingredients(d))), new TableDataGetter<ThingDef>("work amount", (Func<ThingDef, string>)((ThingDef d) => workAmount(d).ToString("F0"))), new TableDataGetter<ThingDef>("real ingredient cost", (Func<ThingDef, string>)((ThingDef d) => realIngredientCost(d).ToString("F1"))), new TableDataGetter<ThingDef>("real sell price", (Func<ThingDef, string>)((ThingDef d) => realSellPrice(d).ToString("F1"))), new TableDataGetter<ThingDef>("real profit per item", (Func<ThingDef, string>)((ThingDef d) => (realSellPrice(d) - realIngredientCost(d)).ToString("F1"))), new TableDataGetter<ThingDef>("real profit per day's work", (Func<ThingDef, string>)((ThingDef d) => ((float)((realSellPrice(d) - realIngredientCost(d)) / workAmount(d) * 30000.0)).ToString("F1"))), new TableDataGetter<ThingDef>("real buy price", (Func<ThingDef, string>)((ThingDef d) => realBuyPrice(d).ToString("F1"))));
		}

		public static void DoTable_WoolEconomy()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh && d.GetCompProperties<CompProperties_Shearable>() != null
			select d, new TableDataGetter<ThingDef>("animal", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("woolDef", (Func<ThingDef, string>)((ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolDef.defName)), new TableDataGetter<ThingDef>("woolAmount", (Func<ThingDef, string>)((ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolAmount.ToString())), new TableDataGetter<ThingDef>("woolValue", (Func<ThingDef, string>)((ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolDef.BaseMarketValue.ToString("F2"))), new TableDataGetter<ThingDef>("shear interval", (Func<ThingDef, string>)((ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().shearIntervalDays.ToString("F1"))), new TableDataGetter<ThingDef>("value per year", (Func<ThingDef, string>)delegate(ThingDef d)
			{
				CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
				return ((float)(compProperties.woolDef.BaseMarketValue * (float)compProperties.woolAmount * (60.0 / (float)compProperties.shearIntervalDays))).ToString("F0");
			}));
		}

		public static void DoTable_AnimalGrowthEconomy()
		{
			Func<ThingDef, float> gestDays = (Func<ThingDef, float>)delegate(ThingDef d)
			{
				float result;
				if (d.HasComp(typeof(CompEggLayer)))
				{
					CompProperties_EggLayer compProperties2 = d.GetCompProperties<CompProperties_EggLayer>();
					result = compProperties2.eggLayIntervalDays / compProperties2.eggCountRange.Average;
				}
				else
				{
					result = d.race.gestationPeriodDays;
				}
				return result;
			};
			Func<ThingDef, float> nutritionToGestate = (Func<ThingDef, float>)delegate(ThingDef d)
			{
				float num4 = 0f;
				LifeStageAge lifeStageAge3 = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1];
				return num4 + gestDays(d) * lifeStageAge3.def.hungerRateFactor * d.race.baseHungerRate;
			};
			Func<ThingDef, float> babyMeatNut = (Func<ThingDef, float>)delegate(ThingDef d)
			{
				LifeStageAge lifeStageAge2 = d.race.lifeStageAges[0];
				return (float)(d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05000000074505806 * lifeStageAge2.def.bodySizeFactor);
			};
			Func<ThingDef, float> babyMeatNutPerInput = (Func<ThingDef, float>)((ThingDef d) => babyMeatNut(d) / nutritionToGestate(d));
			Func<ThingDef, float> nutritionToAdulthood = (Func<ThingDef, float>)delegate(ThingDef d)
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
			Func<ThingDef, float> adultMeatNutPerInput = (Func<ThingDef, float>)((ThingDef d) => (float)(d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05000000074505806 / nutritionToAdulthood(d)));
			Func<ThingDef, float> bestMeatPerInput = (Func<ThingDef, float>)delegate(ThingDef d)
			{
				float a = babyMeatNutPerInput(d);
				float b = adultMeatNutPerInput(d);
				return Mathf.Max(a, b);
			};
			Func<ThingDef, string> eggNut = (Func<ThingDef, string>)delegate(ThingDef d)
			{
				CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				return (compProperties != null) ? compProperties.eggFertilizedDef.ingestible.nutrition.ToString("F2") : "";
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh
			orderby bestMeatPerInput(d) descending
			select d, new TableDataGetter<ThingDef>("animal", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("hungerRate", (Func<ThingDef, string>)((ThingDef d) => d.race.baseHungerRate.ToString("F2"))), new TableDataGetter<ThingDef>("gestDays", (Func<ThingDef, string>)((ThingDef d) => gestDays(d).ToString("F2"))), new TableDataGetter<ThingDef>("herbiv", (Func<ThingDef, string>)((ThingDef d) => (((int)d.race.foodType & 64) == 0) ? "" : "Y")), new TableDataGetter<ThingDef>("eggs", (Func<ThingDef, string>)((ThingDef d) => (!d.HasComp(typeof(CompEggLayer))) ? "" : d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.ToString())), new TableDataGetter<ThingDef>("|", (Func<ThingDef, string>)((ThingDef d) => "|")), new TableDataGetter<ThingDef>("bodySize", (Func<ThingDef, string>)((ThingDef d) => d.race.baseBodySize.ToString("F2"))), new TableDataGetter<ThingDef>("age Adult", (Func<ThingDef, string>)((ThingDef d) => d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge.ToString("F2"))), new TableDataGetter<ThingDef>("nutrition to adulthood", (Func<ThingDef, string>)((ThingDef d) => nutritionToAdulthood(d).ToString("F2"))), new TableDataGetter<ThingDef>("adult meat-nut", (Func<ThingDef, string>)((ThingDef d) => ((float)(d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05000000074505806)).ToString("F2"))), new TableDataGetter<ThingDef>("adult meat-nut / input-nut", (Func<ThingDef, string>)((ThingDef d) => adultMeatNutPerInput(d).ToString("F3"))), new TableDataGetter<ThingDef>("|", (Func<ThingDef, string>)((ThingDef d) => "|")), new TableDataGetter<ThingDef>("baby size", (Func<ThingDef, string>)((ThingDef d) => (d.race.lifeStageAges[0].def.bodySizeFactor * d.race.baseBodySize).ToString("F2"))), new TableDataGetter<ThingDef>("nutrition to gestate", (Func<ThingDef, string>)((ThingDef d) => nutritionToGestate(d).ToString("F2"))), new TableDataGetter<ThingDef>("egg nut", (Func<ThingDef, string>)((ThingDef d) => eggNut(d))), new TableDataGetter<ThingDef>("baby meat-nut", (Func<ThingDef, string>)((ThingDef d) => babyMeatNut(d).ToString("F2"))), new TableDataGetter<ThingDef>("baby meat-nut / input-nut", (Func<ThingDef, string>)((ThingDef d) => babyMeatNutPerInput(d).ToString("F2"))), new TableDataGetter<ThingDef>("baby wins", (Func<ThingDef, string>)((ThingDef d) => (!(babyMeatNutPerInput(d) > adultMeatNutPerInput(d))) ? "" : "B")));
		}

		public static void DoTable_CropEconomy()
		{
			Func<ThingDef, float> workCost = (Func<ThingDef, float>)delegate(ThingDef d)
			{
				float num = 1.1f;
				num = (float)(num + d.plant.growDays * 1.0);
				return (float)(num + (d.plant.sowWork + d.plant.harvestWork) * 0.006120000034570694);
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant && d.plant.Harvestable && d.plant.Sowable
			orderby d.plant.IsTree
			select d, new TableDataGetter<ThingDef>("plant", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("product", (Func<ThingDef, string>)((ThingDef d) => d.plant.harvestedThingDef.defName)), new TableDataGetter<ThingDef>("grow time", (Func<ThingDef, string>)((ThingDef d) => d.plant.growDays.ToString("F1"))), new TableDataGetter<ThingDef>("work", (Func<ThingDef, string>)((ThingDef d) => (d.plant.sowWork + d.plant.harvestWork).ToString("F0"))), new TableDataGetter<ThingDef>("harvestCount", (Func<ThingDef, string>)((ThingDef d) => d.plant.harvestYield.ToString("F1"))), new TableDataGetter<ThingDef>("work-cost per cycle", (Func<ThingDef, string>)((ThingDef d) => workCost(d).ToString("F2"))), new TableDataGetter<ThingDef>("work-cost per harvestCount", (Func<ThingDef, string>)((ThingDef d) => (workCost(d) / d.plant.harvestYield).ToString("F2"))), new TableDataGetter<ThingDef>("value each", (Func<ThingDef, string>)((ThingDef d) => d.plant.harvestedThingDef.BaseMarketValue.ToString("F2"))), new TableDataGetter<ThingDef>("harvestValueTotal", (Func<ThingDef, string>)((ThingDef d) => (d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue).ToString("F2"))), new TableDataGetter<ThingDef>("profit per growDay", (Func<ThingDef, string>)((ThingDef d) => ((d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue - workCost(d)) / d.plant.growDays).ToString("F2"))), new TableDataGetter<ThingDef>("nutrition per growDay", (Func<ThingDef, string>)((ThingDef d) => (d.plant.harvestedThingDef.ingestible == null) ? "" : (d.plant.harvestYield * d.plant.harvestedThingDef.ingestible.nutrition / d.plant.growDays).ToString("F2"))), new TableDataGetter<ThingDef>("nutrition", (Func<ThingDef, string>)((ThingDef d) => (d.plant.harvestedThingDef.ingestible == null) ? "" : d.plant.harvestedThingDef.ingestible.nutrition.ToString("F2"))));
		}

		public static void DoTable_ItemNutritions()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.IsNutritionGivingIngestible
			orderby d.ingestible.nutrition
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("market value", (Func<ThingDef, string>)((ThingDef d) => d.BaseMarketValue.ToString("F1"))), new TableDataGetter<ThingDef>("nutrition", (Func<ThingDef, string>)((ThingDef d) => d.ingestible.nutrition.ToString("F2"))), new TableDataGetter<ThingDef>("nutrition per value", (Func<ThingDef, string>)((ThingDef d) => (d.ingestible.nutrition / d.BaseMarketValue).ToString("F3"))), new TableDataGetter<ThingDef>("work", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, null).ToString("F0"))));
		}

		public static void DoTable_AllNutritions()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.ingestible != null
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("nutrition", (Func<ThingDef, string>)((ThingDef d) => d.ingestible.nutrition.ToStringPercentEmptyZero("F0"))));
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
			Func<ThingDef, float> workAmountGetter = (Func<ThingDef, float>)((ThingDef d) => (float)((d.recipeMaker != null) ? ((d.recipeMaker.workAmount < 0) ? Mathf.Max(d.GetStatValueAbstract(StatDefOf.WorkToMake, null), d.GetStatValueAbstract(StatDefOf.WorkToBuild, null)) : ((float)d.recipeMaker.workAmount)) : -1.0));
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.BaseMarketValue > 0.0099999997764825821 && d.stackLimit > 1 == stackable
			orderby d.BaseMarketValue
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("base market value", (Func<ThingDef, string>)((ThingDef d) => d.BaseMarketValue.ToString("F1"))), new TableDataGetter<ThingDef>("cost to make", (Func<ThingDef, string>)((ThingDef d) => DataAnalysisTableMaker.CostToMakeString(d, false))), new TableDataGetter<ThingDef>("work to make", (Func<ThingDef, string>)((ThingDef d) => (d.recipeMaker == null) ? "-" : workAmountGetter(d).ToString("F1"))), new TableDataGetter<ThingDef>("profit", (Func<ThingDef, string>)((ThingDef d) => (d.BaseMarketValue - DataAnalysisTableMaker.CostToMake(d, false)).ToString("F1"))), new TableDataGetter<ThingDef>("profit rate", (Func<ThingDef, string>)((ThingDef d) => (d.recipeMaker == null) ? "-" : ((float)((d.BaseMarketValue - DataAnalysisTableMaker.CostToMake(d, false)) / workAmountGetter(d) * 10000.0)).ToString("F0"))));
		}

		public static void DoTable_Stuffs()
		{
			Func<ThingDef, StatDef, string> workGetter = (Func<ThingDef, StatDef, string>)delegate(ThingDef d, StatDef stat)
			{
				string result;
				if (d.stuffProps.statFactors == null)
				{
					result = "";
				}
				else
				{
					StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((Func<StatModifier, bool>)((StatModifier fa) => fa.stat == stat));
					result = ((statModifier != null) ? statModifier.value.ToString() : "");
				}
				return result;
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby d.BaseMarketValue
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("base market value", (Func<ThingDef, string>)((ThingDef d) => d.BaseMarketValue.ToString("F1"))), new TableDataGetter<ThingDef>("fac-WorkToMake", (Func<ThingDef, string>)((ThingDef d) => workGetter(d, StatDefOf.WorkToMake))), new TableDataGetter<ThingDef>("fac-WorkToBuild", (Func<ThingDef, string>)((ThingDef d) => workGetter(d, StatDefOf.WorkToBuild))));
		}

		public static void DoTable_ProductionRecipes()
		{
			Func<RecipeDef, float> trueWork = (Func<RecipeDef, float>)((RecipeDef d) => d.WorkAmountTotal(null));
			Func<RecipeDef, float> cheapestIngredientVal = (Func<RecipeDef, float>)delegate(RecipeDef d)
			{
				float num2 = 0f;
				foreach (IngredientCount ingredient in d.ingredients)
				{
					num2 += ingredient.filter.AllowedThingDefs.Min((Func<ThingDef, float>)((ThingDef td) => td.BaseMarketValue)) * ingredient.GetBaseCount();
				}
				return num2;
			};
			Func<RecipeDef, float> workVal = (Func<RecipeDef, float>)((RecipeDef d) => (float)(trueWork(d) * 0.0099999997764825821));
			Func<RecipeDef, float> cheapestProductsVal = (Func<RecipeDef, float>)delegate(RecipeDef d)
			{
				ThingDef thingDef = d.ingredients.First().filter.AllowedThingDefs.MinBy((Func<ThingDef, float>)((ThingDef td) => td.BaseMarketValue));
				float num = 0f;
				foreach (ThingCountClass product in d.products)
				{
					num += product.thingDef.GetStatValueAbstract(StatDefOf.MarketValue, (!product.thingDef.MadeFromStuff) ? null : thingDef) * (float)product.count;
				}
				return num;
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<RecipeDef>.AllDefs
			where !d.products.NullOrEmpty() && !d.ingredients.NullOrEmpty()
			select d, new TableDataGetter<RecipeDef>("defName", (Func<RecipeDef, string>)((RecipeDef d) => d.defName)), new TableDataGetter<RecipeDef>("work", (Func<RecipeDef, string>)((RecipeDef d) => trueWork(d).ToString("F0"))), new TableDataGetter<RecipeDef>("cheapest ingredients value", (Func<RecipeDef, string>)((RecipeDef d) => cheapestIngredientVal(d).ToString("F1"))), new TableDataGetter<RecipeDef>("work value", (Func<RecipeDef, string>)((RecipeDef d) => workVal(d).ToString("F1"))), new TableDataGetter<RecipeDef>("cheapest products value", (Func<RecipeDef, string>)((RecipeDef d) => cheapestProductsVal(d).ToString("F1"))), new TableDataGetter<RecipeDef>("profit raw", (Func<RecipeDef, string>)((RecipeDef d) => (cheapestProductsVal(d) - cheapestIngredientVal(d)).ToString("F1"))), new TableDataGetter<RecipeDef>("profit with work", (Func<RecipeDef, string>)((RecipeDef d) => (cheapestProductsVal(d) - workVal(d) - cheapestIngredientVal(d)).ToString("F1"))), new TableDataGetter<RecipeDef>("profit per work day", (Func<RecipeDef, string>)((RecipeDef d) => ((float)((cheapestProductsVal(d) - cheapestIngredientVal(d)) * 60000.0 / trueWork(d))).ToString("F0"))));
		}

		private static string CostToMakeString(ThingDef d, bool real = false)
		{
			return (d.recipeMaker != null) ? DataAnalysisTableMaker.CostToMake(d, real).ToString("F1") : "-";
		}

		private static float CostToMake(ThingDef d, bool real = false)
		{
			float result;
			if (d.recipeMaker == null)
			{
				result = d.BaseMarketValue;
			}
			else
			{
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
				result = num;
			}
			return result;
		}

		private static bool RequiresBuying(ThingDef def)
		{
			bool result;
			if (def.costList != null)
			{
				foreach (ThingCountClass cost in def.costList)
				{
					if (DataAnalysisTableMaker.RequiresBuying(cost.thingDef))
					{
						return true;
					}
				}
				result = false;
			}
			else
			{
				result = !DefDatabase<ThingDef>.AllDefs.Any((Func<ThingDef, bool>)((ThingDef d) => d.plant != null && d.plant.harvestedThingDef == def && d.plant.Sowable));
			}
			return result;
		}

		public static void DoTable_RacesBasics()
		{
			Func<PawnKindDef, float> dps = (Func<PawnKindDef, float>)((PawnKindDef d) => DataAnalysisTableMaker.RaceMeleeDpsEstimate(d.race));
			Func<PawnKindDef, float> pointsGuess = (Func<PawnKindDef, float>)delegate(PawnKindDef d)
			{
				float num2 = 15f;
				num2 = (float)(num2 + dps(d) * 10.0);
				num2 *= Mathf.Lerp(1f, (float)(d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null) / 3.0), 0.25f);
				num2 *= d.RaceProps.baseHealthScale;
				num2 *= GenMath.LerpDouble(0.25f, 1f, 1.65f, 1f, Mathf.Clamp(d.RaceProps.baseBodySize, 0.25f, 1f));
				return (float)(num2 * 0.75999999046325684);
			};
			Func<PawnKindDef, float> mktValGuess = (Func<PawnKindDef, float>)delegate(PawnKindDef d)
			{
				float num = 18f;
				num = (float)(num + pointsGuess(d) * 2.7000000476837158);
				if (d.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.None)
				{
					num = (float)(num * 0.5);
					goto IL_00a4;
				}
				if (d.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Simple)
				{
					num = (float)(num * 0.800000011920929);
					goto IL_00a4;
				}
				if (d.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Intermediate)
				{
					num = num;
					goto IL_00a4;
				}
				if (d.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Advanced)
				{
					num = (float)(num + 250.0);
					goto IL_00a4;
				}
				throw new InvalidOperationException();
				IL_00a4:
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
			select d, new TableDataGetter<PawnKindDef>("defName", (Func<PawnKindDef, string>)((PawnKindDef d) => d.defName)), new TableDataGetter<PawnKindDef>("points", (Func<PawnKindDef, string>)((PawnKindDef d) => d.combatPower.ToString("F0"))), new TableDataGetter<PawnKindDef>("points guess", (Func<PawnKindDef, string>)((PawnKindDef d) => pointsGuess(d).ToString("F0"))), new TableDataGetter<PawnKindDef>("mktval", (Func<PawnKindDef, string>)((PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"))), new TableDataGetter<PawnKindDef>("mktval guess", (Func<PawnKindDef, string>)((PawnKindDef d) => mktValGuess(d).ToString("F0"))), new TableDataGetter<PawnKindDef>("healthScale", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.baseHealthScale.ToString("F2"))), new TableDataGetter<PawnKindDef>("bodySize", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.baseBodySize.ToString("F2"))), new TableDataGetter<PawnKindDef>("hunger rate", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.baseHungerRate.ToString("F2"))), new TableDataGetter<PawnKindDef>("speed", (Func<PawnKindDef, string>)((PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString("F2"))), new TableDataGetter<PawnKindDef>("melee dps", (Func<PawnKindDef, string>)((PawnKindDef d) => dps(d).ToString("F2"))), new TableDataGetter<PawnKindDef>("wildness", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.wildness.ToStringPercent())), new TableDataGetter<PawnKindDef>("life expec.", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.lifeExpectancy.ToString("F1"))), new TableDataGetter<PawnKindDef>("train-int", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.TrainableIntelligence.label)), new TableDataGetter<PawnKindDef>("temps", (Func<PawnKindDef, string>)((PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null).ToString("F0") + ".." + d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null).ToString("F0"))), new TableDataGetter<PawnKindDef>("mateMtb", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.mateMtbHours.ToStringEmptyZero("F0"))), new TableDataGetter<PawnKindDef>("nuzzMtb", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.nuzzleMtbHours.ToStringEmptyZero("F0"))), new TableDataGetter<PawnKindDef>("mhChDam", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.manhunterOnDamageChance.ToStringPercentEmptyZero("F2"))), new TableDataGetter<PawnKindDef>("mhChTam", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.manhunterOnTameFailChance.ToStringPercentEmptyZero("F2"))));
		}

		private static float RaceMeleeDpsEstimate(ThingDef race)
		{
			float result;
			if (race.Verbs.NullOrEmpty())
			{
				result = 0f;
			}
			else
			{
				IEnumerable<VerbProperties> list = from v in race.Verbs
				where (float)v.meleeDamageBaseAmount > 0.0010000000474974513
				select v;
				result = list.AverageWeighted((Func<VerbProperties, float>)((VerbProperties v) => v.BaseMeleeSelectionWeight), (Func<VerbProperties, float>)((VerbProperties v) => (float)v.meleeDamageBaseAmount / (v.defaultCooldownTime + v.warmupTime)));
			}
			return result;
		}

		public static void DoTable_RacesFoodConsumption()
		{
			Func<ThingDef, int, string> lsName = (Func<ThingDef, int, string>)delegate(ThingDef d, int lsIndex)
			{
				string result3;
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					result3 = "";
				}
				else
				{
					LifeStageDef def3 = d.race.lifeStageAges[lsIndex].def;
					result3 = def3.defName;
				}
				return result3;
			};
			Func<ThingDef, int, string> maxFood = (Func<ThingDef, int, string>)delegate(ThingDef d, int lsIndex)
			{
				string result2;
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					result2 = "";
				}
				else
				{
					LifeStageDef def2 = d.race.lifeStageAges[lsIndex].def;
					result2 = (d.race.baseBodySize * def2.bodySizeFactor * def2.foodMaxFactor).ToString("F2");
				}
				return result2;
			};
			Func<ThingDef, int, string> hungerRate = (Func<ThingDef, int, string>)delegate(ThingDef d, int lsIndex)
			{
				string result;
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					result = "";
				}
				else
				{
					LifeStageDef def = d.race.lifeStageAges[lsIndex].def;
					result = (d.race.baseHungerRate * def.hungerRateFactor).ToString("F2");
				}
				return result;
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null && d.race.EatsFood
			orderby d.race.baseHungerRate descending
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("Lifestage 0", (Func<ThingDef, string>)((ThingDef d) => lsName(d, 0))), new TableDataGetter<ThingDef>("maxFood", (Func<ThingDef, string>)((ThingDef d) => maxFood(d, 0))), new TableDataGetter<ThingDef>("hungerRate", (Func<ThingDef, string>)((ThingDef d) => hungerRate(d, 0))), new TableDataGetter<ThingDef>("Lifestage 1", (Func<ThingDef, string>)((ThingDef d) => lsName(d, 1))), new TableDataGetter<ThingDef>("maxFood", (Func<ThingDef, string>)((ThingDef d) => maxFood(d, 1))), new TableDataGetter<ThingDef>("hungerRate", (Func<ThingDef, string>)((ThingDef d) => hungerRate(d, 1))), new TableDataGetter<ThingDef>("Lifestage 2", (Func<ThingDef, string>)((ThingDef d) => lsName(d, 2))), new TableDataGetter<ThingDef>("maxFood", (Func<ThingDef, string>)((ThingDef d) => maxFood(d, 2))), new TableDataGetter<ThingDef>("hungerRate", (Func<ThingDef, string>)((ThingDef d) => hungerRate(d, 2))), new TableDataGetter<ThingDef>("Lifestage 3", (Func<ThingDef, string>)((ThingDef d) => lsName(d, 3))), new TableDataGetter<ThingDef>("maxFood", (Func<ThingDef, string>)((ThingDef d) => maxFood(d, 3))), new TableDataGetter<ThingDef>("hungerRate", (Func<ThingDef, string>)((ThingDef d) => hungerRate(d, 3))));
		}

		public static void DoTable_AnimalBiomeCommonalities()
		{
			List<TableDataGetter<PawnKindDef>> list = (from b in DefDatabase<BiomeDef>.AllDefs
			where b.implemented && b.canBuildBase
			orderby b.animalDensity
			select new TableDataGetter<PawnKindDef>(b.defName, (Func<PawnKindDef, string>)delegate(PawnKindDef k)
			{
				float num = DefDatabase<PawnKindDef>.AllDefs.Sum((Func<PawnKindDef, float>)((PawnKindDef ki) => b.CommonalityOfAnimal(ki)));
				float num2 = b.CommonalityOfAnimal(k);
				float num3 = num2 / num;
				return (num3 != 0.0) ? num3.ToStringPercent("F1") : "";
			})).ToList();
			list.Insert(0, new TableDataGetter<PawnKindDef>("animal", (Func<PawnKindDef, string>)((PawnKindDef k) => k.defName + ((!k.race.race.predator) ? "" : "*"))));
			DebugTables.MakeTablesDialog(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.defName
			select d, list.ToArray());
		}

		public static void DoTable_AnimalCombatBalance()
		{
			Func<PawnKindDef, float> meleeDps = (Func<PawnKindDef, float>)delegate(PawnKindDef k)
			{
				Pawn pawn2 = PawnGenerator.GeneratePawn(k, null);
				while (pawn2.health.hediffSet.hediffs.Count > 0)
				{
					pawn2.health.RemoveHediff(pawn2.health.hediffSet.hediffs[0]);
				}
				return pawn2.GetStatValue(StatDefOf.MeleeDPS, true);
			};
			Func<PawnKindDef, float> averageArmor = (Func<PawnKindDef, float>)delegate(PawnKindDef k)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(k, null);
				while (pawn.health.hediffSet.hediffs.Count > 0)
				{
					pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
				}
				return (float)((pawn.GetStatValue(StatDefOf.ArmorRating_Blunt, true) + pawn.GetStatValue(StatDefOf.ArmorRating_Sharp, true)) / 2.0);
			};
			Func<PawnKindDef, float> combatPowerCalculated = (Func<PawnKindDef, float>)delegate(PawnKindDef k)
			{
				float num = (float)(1.0 + meleeDps(k) * 2.0);
				float num2 = (float)(1.0 + (k.RaceProps.baseHealthScale + averageArmor(k) * 1.7999999523162842) * 2.0);
				float num3 = (float)(num * num2 * 2.5 + 10.0);
				return (float)(num3 + k.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null) * 2.0);
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.combatPower
			select d, new TableDataGetter<PawnKindDef>("animal", (Func<PawnKindDef, string>)((PawnKindDef k) => k.defName)), new TableDataGetter<PawnKindDef>("meleeDps", (Func<PawnKindDef, string>)((PawnKindDef k) => meleeDps(k).ToString("F1"))), new TableDataGetter<PawnKindDef>("baseHealthScale", (Func<PawnKindDef, string>)((PawnKindDef k) => k.RaceProps.baseHealthScale.ToString())), new TableDataGetter<PawnKindDef>("moveSpeed", (Func<PawnKindDef, string>)((PawnKindDef k) => k.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString())), new TableDataGetter<PawnKindDef>("averageArmor", (Func<PawnKindDef, string>)((PawnKindDef k) => averageArmor(k).ToStringPercent())), new TableDataGetter<PawnKindDef>("combatPowerCalculated", (Func<PawnKindDef, string>)((PawnKindDef k) => combatPowerCalculated(k).ToString("F0"))), new TableDataGetter<PawnKindDef>("combatPower", (Func<PawnKindDef, string>)((PawnKindDef k) => k.combatPower.ToString())));
		}

		public static void DoTable_PlantsBasics()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.plant.fertilitySensitivity
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("growDays", (Func<ThingDef, string>)((ThingDef d) => d.plant.growDays.ToString("F2"))), new TableDataGetter<ThingDef>("reproduceMtb", (Func<ThingDef, string>)((ThingDef d) => d.plant.reproduceMtbDays.ToString("F2"))), new TableDataGetter<ThingDef>("nutrition", (Func<ThingDef, string>)((ThingDef d) => (d.ingestible == null) ? "-" : d.ingestible.nutrition.ToString("F2"))), new TableDataGetter<ThingDef>("nut/day", (Func<ThingDef, string>)((ThingDef d) => (d.ingestible == null) ? "-" : (d.ingestible.nutrition / d.plant.growDays).ToString("F4"))), new TableDataGetter<ThingDef>("fertilityMin", (Func<ThingDef, string>)((ThingDef d) => d.plant.fertilityMin.ToString("F2"))), new TableDataGetter<ThingDef>("fertilitySensitivity", (Func<ThingDef, string>)((ThingDef d) => d.plant.fertilitySensitivity.ToString("F2"))), new TableDataGetter<ThingDef>("blightable", (Func<ThingDef, string>)((ThingDef d) => (!d.plant.Blightable) ? "" : "blightable")));
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
			Func<ThingDef, int> damage = (Func<ThingDef, int>)((ThingDef d) => (d.Verbs[0].defaultProjectile != null) ? d.Verbs[0].defaultProjectile.projectile.damageAmountBase : 0);
			Func<ThingDef, float> warmup = (Func<ThingDef, float>)((ThingDef d) => d.Verbs[0].warmupTime);
			Func<ThingDef, float> cooldown = (Func<ThingDef, float>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, null));
			Func<ThingDef, int> burstShots = (Func<ThingDef, int>)((ThingDef d) => d.Verbs[0].burstShotCount);
			Func<ThingDef, float> dpsMissless = (Func<ThingDef, float>)delegate(ThingDef d)
			{
				int num2 = burstShots(d);
				float num3 = warmup(d) + cooldown(d);
				num3 = (float)(num3 + (float)(num2 - 1) * ((float)d.Verbs[0].ticksBetweenBurstShots / 60.0));
				return (float)(damage(d) * num2) / num3;
			};
			Func<ThingDef, float> accTouch = (Func<ThingDef, float>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyTouch, null));
			Func<ThingDef, float> accShort = (Func<ThingDef, float>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyShort, null));
			Func<ThingDef, float> accMed = (Func<ThingDef, float>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyMedium, null));
			Func<ThingDef, float> accLong = (Func<ThingDef, float>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyLong, null));
			Func<ThingDef, float> dpsAvg = (Func<ThingDef, float>)delegate(ThingDef d)
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
			select d).OrderByDescending(dpsAvg), new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("damage", (Func<ThingDef, string>)((ThingDef d) => damage(d).ToString())), new TableDataGetter<ThingDef>("warmup", (Func<ThingDef, string>)((ThingDef d) => warmup(d).ToString("F2"))), new TableDataGetter<ThingDef>("burst", (Func<ThingDef, string>)((ThingDef d) => burstShots(d).ToString())), new TableDataGetter<ThingDef>("cooldown", (Func<ThingDef, string>)((ThingDef d) => cooldown(d).ToString("F2"))), new TableDataGetter<ThingDef>("range", (Func<ThingDef, string>)((ThingDef d) => d.Verbs[0].range.ToString("F0"))), new TableDataGetter<ThingDef>("dpsMissless", (Func<ThingDef, string>)((ThingDef d) => dpsMissless(d).ToString("F2"))), new TableDataGetter<ThingDef>("accTouch", (Func<ThingDef, string>)((ThingDef d) => accTouch(d).ToStringPercent())), new TableDataGetter<ThingDef>("accShort", (Func<ThingDef, string>)((ThingDef d) => accShort(d).ToStringPercent())), new TableDataGetter<ThingDef>("accMed", (Func<ThingDef, string>)((ThingDef d) => accMed(d).ToStringPercent())), new TableDataGetter<ThingDef>("accLong", (Func<ThingDef, string>)((ThingDef d) => accLong(d).ToStringPercent())), new TableDataGetter<ThingDef>("dpsTouch", (Func<ThingDef, string>)((ThingDef d) => (dpsMissless(d) * accTouch(d)).ToString("F2"))), new TableDataGetter<ThingDef>("dpsShort", (Func<ThingDef, string>)((ThingDef d) => (dpsMissless(d) * accShort(d)).ToString("F2"))), new TableDataGetter<ThingDef>("dpsMed", (Func<ThingDef, string>)((ThingDef d) => (dpsMissless(d) * accMed(d)).ToString("F2"))), new TableDataGetter<ThingDef>("dpsLong", (Func<ThingDef, string>)((ThingDef d) => (dpsMissless(d) * accLong(d)).ToString("F2"))), new TableDataGetter<ThingDef>("dpsAvg", (Func<ThingDef, string>)((ThingDef d) => dpsAvg(d).ToString("F2"))), new TableDataGetter<ThingDef>("mktVal", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"))), new TableDataGetter<ThingDef>("work", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, null).ToString("F0"))), new TableDataGetter<ThingDef>("mktVal/dpsAvg", (Func<ThingDef, string>)((ThingDef d) => (d.GetStatValueAbstract(StatDefOf.MarketValue, null) / dpsAvg(d)).ToString("F2"))));
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
			Func<Def, float> meleeDamageGetter = (Func<Def, float>)delegate(Def d)
			{
				Thing owner2;
				List<Verb> concreteExampleVerbs5 = VerbUtility.GetConcreteExampleVerbs(d, out owner2, stuff);
				return (float)((!concreteExampleVerbs5.OfType<Verb_MeleeAttack>().Any()) ? -1.0 : concreteExampleVerbs5.OfType<Verb_MeleeAttack>().AverageWeighted((Func<Verb_MeleeAttack, float>)((Verb_MeleeAttack v) => v.verbProps.AdjustedMeleeSelectionWeight(v, null, owner2)), (Func<Verb_MeleeAttack, float>)((Verb_MeleeAttack v) => v.verbProps.AdjustedMeleeDamageAmount(v, null, owner2))));
			};
			Func<Def, float> rangedDamageGetter = (Func<Def, float>)delegate(Def d)
			{
				Thing thing3 = default(Thing);
				List<Verb> concreteExampleVerbs4 = VerbUtility.GetConcreteExampleVerbs(d, out thing3, stuff);
				Verb verb3 = concreteExampleVerbs4.OfType<Verb_LaunchProjectile>().FirstOrDefault();
				return (float)((verb3 == null || verb3.GetProjectile() == null) ? -1.0 : ((float)verb3.GetProjectile().projectile.damageAmountBase));
			};
			Func<Def, float> meleeWarmupGetter = (Func<Def, float>)((Def d) => 0f);
			Func<Def, float> rangedWarmupGetter = (Func<Def, float>)delegate(Def d)
			{
				Thing thing2 = default(Thing);
				List<Verb> concreteExampleVerbs3 = VerbUtility.GetConcreteExampleVerbs(d, out thing2, stuff);
				Verb verb2 = concreteExampleVerbs3.OfType<Verb_LaunchProjectile>().FirstOrDefault();
				return (float)((verb2 == null) ? -1.0 : verb2.verbProps.warmupTime);
			};
			Func<Def, float> meleeCooldownGetter = (Func<Def, float>)delegate(Def d)
			{
				Thing owner;
				List<Verb> concreteExampleVerbs2 = VerbUtility.GetConcreteExampleVerbs(d, out owner, stuff);
				return (float)((!concreteExampleVerbs2.OfType<Verb_MeleeAttack>().Any()) ? -1.0 : concreteExampleVerbs2.OfType<Verb_MeleeAttack>().AverageWeighted((Func<Verb_MeleeAttack, float>)((Verb_MeleeAttack v) => v.verbProps.AdjustedMeleeSelectionWeight(v, null, owner)), (Func<Verb_MeleeAttack, float>)((Verb_MeleeAttack v) => v.verbProps.AdjustedCooldown(v, null, owner))));
			};
			Func<Def, float> rangedCooldownGetter = (Func<Def, float>)delegate(Def d)
			{
				Thing thing = default(Thing);
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out thing, stuff);
				Verb verb = concreteExampleVerbs.OfType<Verb_LaunchProjectile>().FirstOrDefault();
				return (float)((verb == null) ? -1.0 : verb.verbProps.defaultCooldownTime);
			};
			Func<Def, float> meleeDpsGetter = (Func<Def, float>)((Def d) => meleeDamageGetter(d) / (meleeWarmupGetter(d) + meleeCooldownGetter(d)));
			Func<Def, float> rangedDpsGetter = (Func<Def, float>)((Def d) => rangedDamageGetter(d) / (rangedWarmupGetter(d) + rangedCooldownGetter(d)));
			Func<Def, float> dpsGetter = (Func<Def, float>)((Def d) => Mathf.Max(meleeDpsGetter(d), rangedDpsGetter(d)));
			Func<Def, float> marketValueGetter = (Func<Def, float>)delegate(Def d)
			{
				ThingDef thingDef2 = d as ThingDef;
				float result;
				if (thingDef2 != null)
				{
					result = thingDef2.GetStatValueAbstract(StatDefOf.MarketValue, stuff);
				}
				else
				{
					HediffDef hediffDef = d as HediffDef;
					result = (float)((hediffDef == null) ? -1.0 : ((hediffDef.spawnThingOnRemoved != null) ? hediffDef.spawnThingOnRemoved.GetStatValueAbstract(StatDefOf.MarketValue, null) : 0.0));
				}
				return result;
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
			DebugTables.MakeTablesDialog(enumerable, new TableDataGetter<Def>("defName", (Func<Def, string>)((Def d) => d.defName)), new TableDataGetter<Def>("mDamage", (Func<Def, string>)((Def d) => meleeDamageGetter(d).ToString())), new TableDataGetter<Def>("mWarmup", (Func<Def, string>)((Def d) => meleeWarmupGetter(d).ToString("F2"))), new TableDataGetter<Def>("mCooldown", (Func<Def, string>)((Def d) => meleeCooldownGetter(d).ToString("F2"))), new TableDataGetter<Def>("mDps", (Func<Def, string>)((Def d) => meleeDpsGetter(d).ToString("F2"))), new TableDataGetter<Def>("rDamage", (Func<Def, string>)((Def d) => rangedDamageGetter(d).ToString())), new TableDataGetter<Def>("rWarmup", (Func<Def, string>)((Def d) => rangedWarmupGetter(d).ToString("F2"))), new TableDataGetter<Def>("rCooldown", (Func<Def, string>)((Def d) => rangedCooldownGetter(d).ToString("F2"))), new TableDataGetter<Def>("rDps", (Func<Def, string>)((Def d) => rangedDpsGetter(d).ToString("F2"))), new TableDataGetter<Def>("dps", (Func<Def, string>)((Def d) => dpsGetter(d).ToString("F2"))), new TableDataGetter<Def>("mktval", (Func<Def, string>)((Def d) => marketValueGetter(d).ToString("F0"))), new TableDataGetter<Def>("work", (Func<Def, string>)delegate(Def d)
			{
				ThingDef thingDef = d as ThingDef;
				return (thingDef != null) ? thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0") : "-";
			}));
		}

		public static void DoTable_WeaponUsage()
		{
			List<TableDataGetter<PawnKindDef>> list = new List<TableDataGetter<PawnKindDef>>();
			list.Add(new TableDataGetter<PawnKindDef>("defName", (Func<PawnKindDef, string>)((PawnKindDef x) => x.defName)));
			list.AddRange(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsWeapon && !x.weaponTags.NullOrEmpty() && x.canBeSpawningInventory
			orderby x.IsMeleeWeapon descending, x.techLevel, x.BaseMarketValue
			select new TableDataGetter<PawnKindDef>(GenText.WithoutVowelsIfLong(x.label), (Func<PawnKindDef, string>)((PawnKindDef y) => (!x.weaponTags.Any((Predicate<string>)((string z) => y.weaponTags.Contains(z)))) ? "" : ((!(y.weaponMoney.max < PawnWeaponGenerator.CheapestNonDerpPriceFor(x))) ? "   ✓" : "  no $"))));
			list.Add(new TableDataGetter<PawnKindDef>("avg $", (Func<PawnKindDef, string>)((PawnKindDef x) => x.weaponMoney.Average.ToString())));
			list.Add(new TableDataGetter<PawnKindDef>("min $", (Func<PawnKindDef, string>)((PawnKindDef x) => x.weaponMoney.min.ToString())));
			list.Add(new TableDataGetter<PawnKindDef>("max $", (Func<PawnKindDef, string>)((PawnKindDef x) => x.weaponMoney.max.ToString())));
			list.Add(new TableDataGetter<PawnKindDef>("points", (Func<PawnKindDef, string>)((PawnKindDef x) => x.combatPower.ToString())));
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
				list.Add(new FloatMenuOption(kind.defName, (Action)delegate()
				{
					Faction faction = FactionUtility.DefaultFactionFrom(kind.defaultFactionType);
					DefMap<ThingDef, int> appCounts = new DefMap<ThingDef, int>();
					for (int i = 0; i < 200; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(kind, faction);
						foreach (Apparel item in pawn.apparel.WornApparel)
						{
							DefMap<ThingDef, int> defMap;
							ThingDef def;
							(defMap = appCounts)[def = item.def] = defMap[def] + 1;
						}
					}
					DebugTables.MakeTablesDialog(DefDatabase<ThingDef>.AllDefs.Where((Func<ThingDef, bool>)((ThingDef d) => d.IsApparel && appCounts[d] > 0)).OrderByDescending((Func<ThingDef, int>)((ThingDef d) => appCounts[d])), new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("count of " + 200, (Func<ThingDef, string>)((ThingDef d) => appCounts[d].ToString())), new TableDataGetter<ThingDef>("percent of pawns", (Func<ThingDef, string>)((ThingDef d) => ((float)((float)appCounts[d] / 200.0)).ToStringPercent("F2"))));
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
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("bodyParts", (Func<ThingDef, string>)((ThingDef d) => GenText.ToSpaceList(d.apparel.bodyPartGroups.Select((Func<BodyPartGroupDef, string>)((BodyPartGroupDef bp) => bp.defName))))), new TableDataGetter<ThingDef>("layers", (Func<ThingDef, string>)((ThingDef d) => GenText.ToSpaceList(d.apparel.layers.Select((Func<ApparelLayer, string>)((ApparelLayer l) => l.ToString()))))), new TableDataGetter<ThingDef>("tags", (Func<ThingDef, string>)((ThingDef d) => GenText.ToSpaceList(d.apparel.tags.Select((Func<string, string>)((string t) => t.ToString()))))), new TableDataGetter<ThingDef>("insCold", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff).ToString("F0"))), new TableDataGetter<ThingDef>("insHeat", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff).ToString("F0"))), new TableDataGetter<ThingDef>("blunt", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuff).ToString("F2"))), new TableDataGetter<ThingDef>("sharp", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuff).ToString("F2"))), new TableDataGetter<ThingDef>("mktval", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, stuff).ToString("F0"))), new TableDataGetter<ThingDef>("work", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0"))));
		}

		public static void DoTable_HitPoints()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.useHitPoints
			orderby d.GetStatValueAbstract(StatDefOf.MaxHitPoints, null) descending
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("hp", (Func<ThingDef, string>)((ThingDef d) => d.BaseMaxHitPoints.ToString())), new TableDataGetter<ThingDef>("category", (Func<ThingDef, string>)((ThingDef d) => d.category.ToString())));
		}

		public static void DoTable_FillPercent()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.fillPercent > 0.0
			orderby d.fillPercent descending
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("fillPercent", (Func<ThingDef, string>)((ThingDef d) => d.fillPercent.ToStringPercent())), new TableDataGetter<ThingDef>("category", (Func<ThingDef, string>)((ThingDef d) => d.category.ToString())));
		}

		public static void DoTable_DeteriorationRates()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) > 0.0
			orderby d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) descending
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("deterioration rate", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null).ToString("F1"))), new TableDataGetter<ThingDef>("hp", (Func<ThingDef, string>)((ThingDef d) => d.BaseMaxHitPoints.ToString())), new TableDataGetter<ThingDef>("days to vanish", (Func<ThingDef, string>)((ThingDef d) => ((float)d.BaseMaxHitPoints / d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null)).ToString("0.#"))));
		}

		public static void DoTable_ShootingAccuracy()
		{
			StatDef stat = StatDefOf.ShootingAccuracy;
			Func<int, float, int, float> accAtDistance = (Func<int, float, int, float>)delegate(int level, float dist, int traitDegree)
			{
				float num = 1f;
				if (traitDegree != 0)
				{
					float value = TraitDef.Named("ShootingAccuracy").DataAtDegree(traitDegree).statOffsets.First((Func<StatModifier, bool>)((StatModifier so) => so.stat == stat)).value;
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
			DebugTables.MakeTablesDialog(list, new TableDataGetter<int>("No trait skill", (Func<int, string>)((int lev) => lev.ToString())), new TableDataGetter<int>("acc at 1", (Func<int, string>)((int lev) => accAtDistance(lev, 1f, 0).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 10", (Func<int, string>)((int lev) => accAtDistance(lev, 10f, 0).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 20", (Func<int, string>)((int lev) => accAtDistance(lev, 20f, 0).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 30", (Func<int, string>)((int lev) => accAtDistance(lev, 30f, 0).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 50", (Func<int, string>)((int lev) => accAtDistance(lev, 50f, 0).ToStringPercent("F2"))), new TableDataGetter<int>("Careful shooter skill", (Func<int, string>)((int lev) => lev.ToString())), new TableDataGetter<int>("acc at 1", (Func<int, string>)((int lev) => accAtDistance(lev, 1f, 1).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 10", (Func<int, string>)((int lev) => accAtDistance(lev, 10f, 1).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 20", (Func<int, string>)((int lev) => accAtDistance(lev, 20f, 1).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 30", (Func<int, string>)((int lev) => accAtDistance(lev, 30f, 1).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 50", (Func<int, string>)((int lev) => accAtDistance(lev, 50f, 1).ToStringPercent("F2"))), new TableDataGetter<int>("Trigger-happy skill", (Func<int, string>)((int lev) => lev.ToString())), new TableDataGetter<int>("acc at 1", (Func<int, string>)((int lev) => accAtDistance(lev, 1f, -1).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 10", (Func<int, string>)((int lev) => accAtDistance(lev, 10f, -1).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 20", (Func<int, string>)((int lev) => accAtDistance(lev, 20f, -1).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 30", (Func<int, string>)((int lev) => accAtDistance(lev, 30f, -1).ToStringPercent("F2"))), new TableDataGetter<int>("acc at 50", (Func<int, string>)((int lev) => accAtDistance(lev, 50f, -1).ToStringPercent("F2"))));
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
				list.Add(new FloatMenuOption(localBd.defName, (Action)delegate()
				{
					DebugTables.MakeTablesDialog(from d in localBd.AllParts
					orderby d.height descending
					select d, new TableDataGetter<BodyPartRecord>("defName", (Func<BodyPartRecord, string>)((BodyPartRecord d) => d.def.defName)), new TableDataGetter<BodyPartRecord>("coverage", (Func<BodyPartRecord, string>)((BodyPartRecord d) => d.coverage.ToStringPercent())), new TableDataGetter<BodyPartRecord>("coverageAbsWithChildren", (Func<BodyPartRecord, string>)((BodyPartRecord d) => d.coverageAbsWithChildren.ToStringPercent())), new TableDataGetter<BodyPartRecord>("coverageAbs", (Func<BodyPartRecord, string>)((BodyPartRecord d) => d.coverageAbs.ToStringPercent())), new TableDataGetter<BodyPartRecord>("depth", (Func<BodyPartRecord, string>)((BodyPartRecord d) => d.depth.ToString())), new TableDataGetter<BodyPartRecord>("height", (Func<BodyPartRecord, string>)((BodyPartRecord d) => d.height.ToString())));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		private static void DoTable_FillPercents(ThingCategory cat)
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == cat && !d.IsFrame && d.passability != Traversability.Impassable
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("fillPercent", (Func<ThingDef, string>)((ThingDef d) => d.fillPercent.ToStringPercent())));
		}

		public static void DoTable_ThingMasses()
		{
			IOrderedEnumerable<ThingDef> dataSources = from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Item || x.Minifiable
			where x.thingClass != typeof(MinifiedThing) && x.thingClass != typeof(UnfinishedThing)
			orderby x.GetStatValueAbstract(StatDefOf.Mass, null), x.GetStatValueAbstract(StatDefOf.MarketValue, null)
			select x;
			Func<ThingDef, float, string> perPawn = (Func<ThingDef, float, string>)((ThingDef d, float bodySize) => ((float)(bodySize * 35.0 / d.GetStatValueAbstract(StatDefOf.Mass, null))).ToString("F0"));
			Func<ThingDef, string> perNutrition = (Func<ThingDef, string>)((ThingDef d) => (d.ingestible != null && d.ingestible.nutrition != 0.0) ? (d.GetStatValueAbstract(StatDefOf.Mass, null) / d.ingestible.nutrition).ToString("F2") : "");
			DebugTables.MakeTablesDialog(dataSources, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)delegate(ThingDef d)
			{
				string result;
				if (d.Minifiable)
				{
					result = d.defName + " (minified)";
				}
				else
				{
					string text = d.defName;
					if (!d.EverHaulable)
					{
						text += " (not haulable)";
					}
					result = text;
				}
				return result;
			}), new TableDataGetter<ThingDef>("mass", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.Mass, null).ToString())), new TableDataGetter<ThingDef>("per human", (Func<ThingDef, string>)((ThingDef d) => perPawn(d, ThingDefOf.Human.race.baseBodySize))), new TableDataGetter<ThingDef>("per muffalo", (Func<ThingDef, string>)((ThingDef d) => perPawn(d, ThingDefOf.Muffalo.race.baseBodySize))), new TableDataGetter<ThingDef>("per dromedary", (Func<ThingDef, string>)((ThingDef d) => perPawn(d, ThingDefOf.Dromedary.race.baseBodySize))), new TableDataGetter<ThingDef>("per nutrition", (Func<ThingDef, string>)((ThingDef d) => perNutrition(d))), new TableDataGetter<ThingDef>("small volume", (Func<ThingDef, string>)((ThingDef d) => (!d.smallVolume) ? "" : "small")));
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
				new TableDataGetter<float>("potency", (Func<float, string>)((float p) => p.ToStringPercent())),
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
				array[i + 1] = new TableDataGetter<float>((i + 1).ToString(), (Func<float, string>)((float p) => (p * factor).ToStringPercent()));
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
			list.Add(new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)));
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
				list.Add(new TableDataGetter<ThingDef>(defName, (Func<ThingDef, string>)((ThingDef td) => (!localTk.WillTrade(td)) ? "" : "✓")));
			}
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.0010000000474974513 && !d.isUnfinishedThing && !d.IsCorpse && !d.destroyOnDrop && d != ThingDefOf.Silver && !d.thingCategories.NullOrEmpty()) || (d.category == ThingCategory.Building && d.Minifiable)
			orderby d.thingCategories.NullOrEmpty() ? "zzzzzzz" : d.thingCategories[0].defName, d.BaseMarketValue
			select d, list.ToArray());
		}

		public static void DoTable_Surgeries()
		{
			Func<RecipeDef, float> trueWork = (Func<RecipeDef, float>)((RecipeDef d) => d.WorkAmountTotal(null));
			DebugTables.MakeTablesDialog((from d in DefDatabase<RecipeDef>.AllDefs
			where d.IsSurgery
			select d).OrderByDescending(trueWork), new TableDataGetter<RecipeDef>("defName", (Func<RecipeDef, string>)((RecipeDef d) => d.defName)), new TableDataGetter<RecipeDef>("work", (Func<RecipeDef, string>)((RecipeDef d) => trueWork(d).ToString("F0"))), new TableDataGetter<RecipeDef>("ingredients", (Func<RecipeDef, string>)((RecipeDef d) => GenText.ToCommaList(from ing in d.ingredients
			select ing.ToString(), false))), new TableDataGetter<RecipeDef>("skillRequirements", (Func<RecipeDef, string>)((RecipeDef d) => (d.skillRequirements != null) ? GenText.ToCommaList(from ing in d.skillRequirements
			select ing.ToString(), false) : "-")), new TableDataGetter<RecipeDef>("surgerySuccessChanceFactor", (Func<RecipeDef, string>)((RecipeDef d) => d.surgerySuccessChanceFactor.ToStringPercent())), new TableDataGetter<RecipeDef>("deathOnFailChance", (Func<RecipeDef, string>)((RecipeDef d) => d.deathOnFailedSurgeryChance.ToStringPercent())));
		}

		public static void DoTable_Terrains()
		{
			DebugTables.MakeTablesDialog(DefDatabase<TerrainDef>.AllDefs, new TableDataGetter<TerrainDef>("defName", (Func<TerrainDef, string>)((TerrainDef d) => d.defName)), new TableDataGetter<TerrainDef>("beauty", (Func<TerrainDef, string>)((TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString())), new TableDataGetter<TerrainDef>("cleanliness", (Func<TerrainDef, string>)((TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Cleanliness, null).ToString())), new TableDataGetter<TerrainDef>("pathCost", (Func<TerrainDef, string>)((TerrainDef d) => d.pathCost.ToString())));
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
			select x, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("best local", (Func<ThingDef, string>)delegate(ThingDef d)
			{
				IEnumerable<ThingRequestGroup> source2 = ListerThings.EverListable(d, ListerThingsUse.Region) ? ((ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))).Where((Func<ThingRequestGroup, bool>)((ThingRequestGroup x) => x.StoreInRegion() && x.Includes(d))) : Enumerable.Empty<ThingRequestGroup>();
				string result2;
				if (!source2.Any())
				{
					result2 = "-";
				}
				else
				{
					ThingRequestGroup best2 = source2.MinBy((Func<ThingRequestGroup, int>)((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((Func<ThingDef, bool>)((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Region) && x.Includes(y)))));
					result2 = best2 + " (defs: " + DefDatabase<ThingDef>.AllDefs.Count((Func<ThingDef, bool>)((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Region) && best2.Includes(x))) + ")";
				}
				return result2;
			}), new TableDataGetter<ThingDef>("best global", (Func<ThingDef, string>)delegate(ThingDef d)
			{
				IEnumerable<ThingRequestGroup> source = ListerThings.EverListable(d, ListerThingsUse.Global) ? ((ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))).Where((Func<ThingRequestGroup, bool>)((ThingRequestGroup x) => x.Includes(d))) : Enumerable.Empty<ThingRequestGroup>();
				string result;
				if (!source.Any())
				{
					result = "-";
				}
				else
				{
					ThingRequestGroup best = source.MinBy((Func<ThingRequestGroup, int>)((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((Func<ThingDef, bool>)((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Global) && x.Includes(y)))));
					result = best + " (defs: " + DefDatabase<ThingDef>.AllDefs.Count((Func<ThingDef, bool>)((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Global) && best.Includes(x))) + ")";
				}
				return result;
			}));
		}

		public static void DoTable_Drugs()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.IsDrug
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("pleasue", (Func<ThingDef, string>)((ThingDef d) => (!d.IsPleasureDrug) ? "" : "pleasure")), new TableDataGetter<ThingDef>("non-medical", (Func<ThingDef, string>)((ThingDef d) => (!d.IsNonMedicalDrug) ? "" : "non-medical")));
		}

		public static void DoTable_PawnGroupsMadeRepeated()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction allFaction in Find.FactionManager.AllFactions)
			{
				if (allFaction.def.pawnGroupMakers != null && allFaction.def.pawnGroupMakers.Any((Predicate<PawnGroupMaker>)((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Normal)))
				{
					Faction localFac = allFaction;
					list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, (Action)delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (float item in Dialog_DebugActionsMenu.PointsOptions())
						{
							float num = item;
							float localP = num;
							list2.Add(new DebugMenuOption(localP.ToString(), DebugMenuOptionMode.Action, (Action)delegate()
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
									foreach (Pawn item in list3)
									{
										if (item.equipment.Primary != null)
										{
											if (!weaponsCount[i].ContainsKey(item.equipment.Primary.def))
											{
												weaponsCount[i].Add(item.equipment.Primary.def, 0);
											}
											Dictionary<ThingDef, int> dictionary;
											ThingDef def;
											(dictionary = weaponsCount[i])[def = item.equipment.Primary.def] = dictionary[def] + 1;
										}
										item.Destroy(DestroyMode.Vanish);
									}
								}
								int totalPawns = weaponsCount.Sum((Func<Dictionary<ThingDef, int>, int>)((Dictionary<ThingDef, int> x) => x.Sum((Func<KeyValuePair<ThingDef, int>, int>)((KeyValuePair<ThingDef, int> y) => y.Value))));
								List<TableDataGetter<int>> list4 = new List<TableDataGetter<int>>();
								list4.Add(new TableDataGetter<int>("", (Func<int, string>)((int x) => (x != 20) ? (x + 1).ToString() : "avg")));
								list4.Add(new TableDataGetter<int>("pawns", (Func<int, string>)((int x) => " " + ((x != 20) ? weaponsCount[x].Sum((Func<KeyValuePair<ThingDef, int>, int>)((KeyValuePair<ThingDef, int> y) => y.Value)).ToString() : ((float)((float)totalPawns / 20.0)).ToString("0.#")))));
								list4.AddRange(from x in DefDatabase<ThingDef>.AllDefs
								where x.IsWeapon && !x.weaponTags.NullOrEmpty() && x.canBeSpawningInventory
								orderby x.IsMeleeWeapon descending, x.techLevel, x.BaseMarketValue
								select new TableDataGetter<int>(GenText.WithoutVowelsIfLong(x.label), (Func<int, string>)((int y) => (y != 20) ? ((!weaponsCount[y].ContainsKey(x)) ? "" : (" " + weaponsCount[y][x] + " (" + ((float)weaponsCount[y][x] / (float)weaponsCount[y].Sum((Func<KeyValuePair<ThingDef, int>, int>)((KeyValuePair<ThingDef, int> z) => z.Value))).ToStringPercent("F0") + ")")) : (" " + ((float)((float)weaponsCount.Sum((Func<Dictionary<ThingDef, int>, int>)((Dictionary<ThingDef, int> z) => z.ContainsKey(x) ? z[x] : 0)) / 20.0)).ToString("0.#")))));
								list4.Add(new TableDataGetter<int>("kinds", (Func<int, string>)((int x) => (x != 20) ? pawnKinds[x] : "")));
								DebugTables.MakeTablesDialog(Enumerable.Range(0, 21), list4.ToArray());
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		private static string ToStringEmptyZero(this float f, string format)
		{
			return (!(f <= 0.0)) ? f.ToString(format) : "";
		}

		private static string ToStringPercentEmptyZero(this float f, string format = "F0")
		{
			return (!(f <= 0.0)) ? f.ToStringPercent(format) : "";
		}

		private static void DoMentalBreaksTable(IEnumerable<MentalBreakDef> breaks)
		{
			float sumWeights = breaks.Sum((Func<MentalBreakDef, float>)((MentalBreakDef x) => x.baseCommonality));
			DebugTables.MakeTablesDialog(breaks, new TableDataGetter<MentalBreakDef>("defName", (Func<MentalBreakDef, string>)((MentalBreakDef d) => d.defName)), new TableDataGetter<MentalBreakDef>("chance", (Func<MentalBreakDef, string>)((MentalBreakDef d) => (d.baseCommonality / sumWeights).ToStringPercent())), new TableDataGetter<MentalBreakDef>("min duration (days)", (Func<MentalBreakDef, string>)((MentalBreakDef d) => (d.mentalState != null) ? ((float)((float)d.mentalState.minTicksBeforeRecovery / 60000.0)).ToString("0.##") : "")), new TableDataGetter<MentalBreakDef>("avg duration (days)", (Func<MentalBreakDef, string>)((MentalBreakDef d) => (d.mentalState != null) ? ((float)(Mathf.Min((float)((float)d.mentalState.minTicksBeforeRecovery + d.mentalState.recoveryMtbDays * 60000.0), (float)d.mentalState.maxTicksBeforeRecovery) / 60000.0)).ToString("0.##") : "")), new TableDataGetter<MentalBreakDef>("max duration (days)", (Func<MentalBreakDef, string>)((MentalBreakDef d) => (d.mentalState != null) ? ((float)((float)d.mentalState.maxTicksBeforeRecovery / 60000.0)).ToString("0.##") : "")), new TableDataGetter<MentalBreakDef>("recoverFromSleep", (Func<MentalBreakDef, string>)((MentalBreakDef d) => (d.mentalState == null || !d.mentalState.recoverFromSleep) ? "" : "recoverFromSleep")), new TableDataGetter<MentalBreakDef>("recoveryThought", (Func<MentalBreakDef, string>)((MentalBreakDef d) => (d.mentalState != null) ? d.mentalState.moodRecoveryThought.ToStringSafe() : "")), new TableDataGetter<MentalBreakDef>("aggro", (Func<MentalBreakDef, string>)((MentalBreakDef d) => (d.mentalState == null || !d.mentalState.IsAggro) ? "" : "aggro")), new TableDataGetter<MentalBreakDef>("blockNormalThoughts", (Func<MentalBreakDef, string>)((MentalBreakDef d) => (d.mentalState == null || !d.mentalState.blockNormalThoughts) ? "" : "blockNormalThoughts")), new TableDataGetter<MentalBreakDef>("allowBeatfire", (Func<MentalBreakDef, string>)((MentalBreakDef d) => (d.mentalState == null || !d.mentalState.allowBeatfire) ? "" : "allowBeatfire")));
		}
	}
}
