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
				if (d.costList == null)
				{
					return "-";
				}
				StringBuilder stringBuilder = new StringBuilder();
				List<ThingCountClass>.Enumerator enumerator = d.costList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ThingCountClass current = enumerator.Current;
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(", ");
						}
						string text = (!DataAnalysisTableMaker.RequiresBuying(current.thingDef)) ? string.Empty : "*";
						stringBuilder.Append(current.thingDef.defName + text + " x" + current.count);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				return stringBuilder.ToString().TrimEndNewlines();
			};
			Func<ThingDef, float> workAmount = (Func<ThingDef, float>)delegate(ThingDef d)
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
				if (d.HasComp(typeof(CompEggLayer)))
				{
					CompProperties_EggLayer compProperties2 = d.GetCompProperties<CompProperties_EggLayer>();
					return compProperties2.eggLayIntervalDays / compProperties2.eggCountRange.Average;
				}
				return d.race.gestationPeriodDays;
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
				if (compProperties == null)
				{
					return string.Empty;
				}
				return compProperties.eggFertilizedDef.ingestible.nutrition.ToString("F2");
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh
			orderby bestMeatPerInput(d) descending
			select d, new TableDataGetter<ThingDef>("animal", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("hungerRate", (Func<ThingDef, string>)((ThingDef d) => d.race.baseHungerRate.ToString("F2"))), new TableDataGetter<ThingDef>("gestDays", (Func<ThingDef, string>)((ThingDef d) => gestDays(d).ToString("F2"))), new TableDataGetter<ThingDef>("herbiv", (Func<ThingDef, string>)((ThingDef d) => (((int)d.race.foodType & 64) == 0) ? string.Empty : "Y")), new TableDataGetter<ThingDef>("eggs", (Func<ThingDef, string>)((ThingDef d) => (!d.HasComp(typeof(CompEggLayer))) ? string.Empty : d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.ToString())), new TableDataGetter<ThingDef>("|", (Func<ThingDef, string>)((ThingDef d) => "|")), new TableDataGetter<ThingDef>("bodySize", (Func<ThingDef, string>)((ThingDef d) => d.race.baseBodySize.ToString("F2"))), new TableDataGetter<ThingDef>("age Adult", (Func<ThingDef, string>)((ThingDef d) => d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge.ToString("F2"))), new TableDataGetter<ThingDef>("nutrition to adulthood", (Func<ThingDef, string>)((ThingDef d) => nutritionToAdulthood(d).ToString("F2"))), new TableDataGetter<ThingDef>("adult meat-nut", (Func<ThingDef, string>)((ThingDef d) => ((float)(d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05000000074505806)).ToString("F2"))), new TableDataGetter<ThingDef>("adult meat-nut / input-nut", (Func<ThingDef, string>)((ThingDef d) => adultMeatNutPerInput(d).ToString("F3"))), new TableDataGetter<ThingDef>("|", (Func<ThingDef, string>)((ThingDef d) => "|")), new TableDataGetter<ThingDef>("baby size", (Func<ThingDef, string>)((ThingDef d) => (d.race.lifeStageAges[0].def.bodySizeFactor * d.race.baseBodySize).ToString("F2"))), new TableDataGetter<ThingDef>("nutrition to gestate", (Func<ThingDef, string>)((ThingDef d) => nutritionToGestate(d).ToString("F2"))), new TableDataGetter<ThingDef>("egg nut", (Func<ThingDef, string>)((ThingDef d) => eggNut(d))), new TableDataGetter<ThingDef>("baby meat-nut", (Func<ThingDef, string>)((ThingDef d) => babyMeatNut(d).ToString("F2"))), new TableDataGetter<ThingDef>("baby meat-nut / input-nut", (Func<ThingDef, string>)((ThingDef d) => babyMeatNutPerInput(d).ToString("F2"))), new TableDataGetter<ThingDef>("baby wins", (Func<ThingDef, string>)((ThingDef d) => (!(babyMeatNutPerInput(d) > adultMeatNutPerInput(d))) ? string.Empty : "B")));
		}

		public static void DoTable_CropEconomy()
		{
			Func<ThingDef, float> calculatedProductionCost = (Func<ThingDef, float>)delegate(ThingDef d)
			{
				float num = 1.1f;
				num = (float)(num + d.plant.growDays * 2.7999999523162842);
				return (float)(num + (d.plant.sowWork + d.plant.harvestWork) * 0.0060000000521540642);
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant && d.plant.Harvestable && d.plant.Sowable && !d.plant.IsTree
			select d, new TableDataGetter<ThingDef>("plant", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("product", (Func<ThingDef, string>)((ThingDef d) => d.plant.harvestedThingDef.defName)), new TableDataGetter<ThingDef>("grow time", (Func<ThingDef, string>)((ThingDef d) => d.plant.growDays.ToString("F1"))), new TableDataGetter<ThingDef>("work", (Func<ThingDef, string>)((ThingDef d) => (d.plant.sowWork + d.plant.harvestWork).ToString("F0"))), new TableDataGetter<ThingDef>("yield", (Func<ThingDef, string>)((ThingDef d) => d.plant.harvestYield.ToString("F1"))), new TableDataGetter<ThingDef>("yield value", (Func<ThingDef, string>)((ThingDef d) => (d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue).ToString("F1"))), new TableDataGetter<ThingDef>("calculated production cost total", (Func<ThingDef, string>)((ThingDef d) => calculatedProductionCost(d).ToString("F2"))), new TableDataGetter<ThingDef>("calculated production cost", (Func<ThingDef, string>)((ThingDef d) => (calculatedProductionCost(d) / d.plant.harvestYield).ToString("F2"))), new TableDataGetter<ThingDef>("value", (Func<ThingDef, string>)((ThingDef d) => d.plant.harvestedThingDef.BaseMarketValue.ToString("F1"))), new TableDataGetter<ThingDef>("nutrition", (Func<ThingDef, string>)((ThingDef d) => (d.plant.harvestedThingDef.ingestible == null) ? string.Empty : d.plant.harvestedThingDef.ingestible.nutrition.ToString("F2"))));
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
			Func<ThingDef, float> workAmountGetter = (Func<ThingDef, float>)delegate(ThingDef d)
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
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("base market value", (Func<ThingDef, string>)((ThingDef d) => d.BaseMarketValue.ToString("F1"))), new TableDataGetter<ThingDef>("cost to make", (Func<ThingDef, string>)((ThingDef d) => DataAnalysisTableMaker.CostToMakeString(d, false))), new TableDataGetter<ThingDef>("work to make", (Func<ThingDef, string>)((ThingDef d) => (d.recipeMaker == null) ? "-" : workAmountGetter(d).ToString("F1"))), new TableDataGetter<ThingDef>("profit", (Func<ThingDef, string>)((ThingDef d) => (d.BaseMarketValue - DataAnalysisTableMaker.CostToMake(d, false)).ToString("F1"))), new TableDataGetter<ThingDef>("profit rate", (Func<ThingDef, string>)((ThingDef d) => (d.recipeMaker == null) ? "-" : ((float)((d.BaseMarketValue - DataAnalysisTableMaker.CostToMake(d, false)) / workAmountGetter(d) * 10000.0)).ToString("F0"))));
		}

		public static void DoTable_Stuffs()
		{
			Func<ThingDef, StatDef, string> workGetter = (Func<ThingDef, StatDef, string>)delegate(ThingDef d, StatDef stat)
			{
				if (d.stuffProps.statFactors == null)
				{
					return string.Empty;
				}
				StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((Func<StatModifier, bool>)((StatModifier fa) => fa.stat == stat));
				if (statModifier == null)
				{
					return string.Empty;
				}
				return statModifier.value.ToString();
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby d.BaseMarketValue
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("base market value", (Func<ThingDef, string>)((ThingDef d) => d.BaseMarketValue.ToString("F1"))), new TableDataGetter<ThingDef>("fac-WorkToMake", (Func<ThingDef, string>)((ThingDef d) => workGetter(d, StatDefOf.WorkToMake))), new TableDataGetter<ThingDef>("fac-WorkToBuild", (Func<ThingDef, string>)((ThingDef d) => workGetter(d, StatDefOf.WorkToBuild))));
		}

		public static void DoTable_Recipes()
		{
			Func<RecipeDef, float> trueWork = (Func<RecipeDef, float>)((RecipeDef d) => d.WorkAmountTotal(null));
			Func<RecipeDef, float> cheapestIngredientVal = (Func<RecipeDef, float>)delegate(RecipeDef d)
			{
				float num2 = 0f;
				List<IngredientCount>.Enumerator enumerator2 = d.ingredients.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						IngredientCount current2 = enumerator2.Current;
						num2 += current2.filter.AllowedThingDefs.Min((Func<ThingDef, float>)((ThingDef td) => td.BaseMarketValue)) * current2.GetBaseCount();
					}
					return num2;
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
			};
			Func<RecipeDef, float> workVal = (Func<RecipeDef, float>)((RecipeDef d) => (float)(trueWork(d) * 0.0099999997764825821));
			Func<RecipeDef, float> cheapestProductsVal = (Func<RecipeDef, float>)delegate(RecipeDef d)
			{
				ThingDef thingDef = d.ingredients.First().filter.AllowedThingDefs.MinBy((Func<ThingDef, float>)((ThingDef td) => td.BaseMarketValue));
				float num = 0f;
				List<ThingCountClass>.Enumerator enumerator = d.products.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ThingCountClass current = enumerator.Current;
						num += current.thingDef.GetStatValueAbstract(StatDefOf.MarketValue, (!current.thingDef.MadeFromStuff) ? null : thingDef) * (float)current.count;
					}
					return num;
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<RecipeDef>.AllDefs
			where !d.products.NullOrEmpty()
			select d, new TableDataGetter<RecipeDef>("defName", (Func<RecipeDef, string>)((RecipeDef d) => d.defName)), new TableDataGetter<RecipeDef>("work", (Func<RecipeDef, string>)((RecipeDef d) => trueWork(d).ToString("F0"))), new TableDataGetter<RecipeDef>("cheapest ingredients value", (Func<RecipeDef, string>)((RecipeDef d) => cheapestIngredientVal(d).ToString("F1"))), new TableDataGetter<RecipeDef>("work value", (Func<RecipeDef, string>)((RecipeDef d) => workVal(d).ToString("F1"))), new TableDataGetter<RecipeDef>("cheapest products value", (Func<RecipeDef, string>)((RecipeDef d) => cheapestProductsVal(d).ToString("F1"))), new TableDataGetter<RecipeDef>("profit raw", (Func<RecipeDef, string>)((RecipeDef d) => (cheapestProductsVal(d) - cheapestIngredientVal(d)).ToString("F1"))), new TableDataGetter<RecipeDef>("profit with work", (Func<RecipeDef, string>)((RecipeDef d) => (cheapestProductsVal(d) - workVal(d) - cheapestIngredientVal(d)).ToString("F1"))), new TableDataGetter<RecipeDef>("profit per work day", (Func<RecipeDef, string>)((RecipeDef d) => ((float)((cheapestProductsVal(d) - cheapestIngredientVal(d)) * 60000.0 / trueWork(d))).ToString("F0"))));
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
				List<ThingCountClass>.Enumerator enumerator = d.costList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ThingCountClass current = enumerator.Current;
						float num2 = 1f;
						if (real)
						{
							num2 = (float)((!DataAnalysisTableMaker.RequiresBuying(current.thingDef)) ? 0.5 : 1.5);
						}
						num += (float)current.count * DataAnalysisTableMaker.CostToMake(current.thingDef, true) * num2;
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
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
				List<ThingCountClass>.Enumerator enumerator = def.costList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ThingCountClass current = enumerator.Current;
						if (DataAnalysisTableMaker.RequiresBuying(current.thingDef))
						{
							return true;
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				return false;
			}
			return !DefDatabase<ThingDef>.AllDefs.Any((Func<ThingDef, bool>)((ThingDef d) => d.plant != null && d.plant.harvestedThingDef == def && d.plant.Sowable));
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
			select d, new TableDataGetter<PawnKindDef>("defName", (Func<PawnKindDef, string>)((PawnKindDef d) => d.defName)), new TableDataGetter<PawnKindDef>("points", (Func<PawnKindDef, string>)((PawnKindDef d) => d.combatPower.ToString("F0"))), new TableDataGetter<PawnKindDef>("points guess", (Func<PawnKindDef, string>)((PawnKindDef d) => pointsGuess(d).ToString("F0"))), new TableDataGetter<PawnKindDef>("mktval", (Func<PawnKindDef, string>)((PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"))), new TableDataGetter<PawnKindDef>("mktval guess", (Func<PawnKindDef, string>)((PawnKindDef d) => mktValGuess(d).ToString("F0"))), new TableDataGetter<PawnKindDef>("healthScale", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.baseHealthScale.ToString("F2"))), new TableDataGetter<PawnKindDef>("bodySize", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.baseBodySize.ToString("F2"))), new TableDataGetter<PawnKindDef>("hunger rate", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.baseHungerRate.ToString("F2"))), new TableDataGetter<PawnKindDef>("speed", (Func<PawnKindDef, string>)((PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString("F2"))), new TableDataGetter<PawnKindDef>("melee dps", (Func<PawnKindDef, string>)((PawnKindDef d) => dps(d).ToString("F2"))), new TableDataGetter<PawnKindDef>("wildness", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.wildness.ToStringPercent())), new TableDataGetter<PawnKindDef>("life expec.", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.lifeExpectancy.ToString("F1"))), new TableDataGetter<PawnKindDef>("train-int", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.TrainableIntelligence.GetLabel())), new TableDataGetter<PawnKindDef>("temps", (Func<PawnKindDef, string>)((PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null).ToString("F0") + ".." + d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null).ToString("F0"))), new TableDataGetter<PawnKindDef>("mateMtb", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.mateMtbHours.ToStringEmptyZero("F0"))), new TableDataGetter<PawnKindDef>("nuzzMtb", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.nuzzleMtbHours.ToStringEmptyZero("F0"))), new TableDataGetter<PawnKindDef>("mhChDam", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.manhunterOnDamageChance.ToStringPercentEmptyZero("F2"))), new TableDataGetter<PawnKindDef>("mhChTam", (Func<PawnKindDef, string>)((PawnKindDef d) => d.RaceProps.manhunterOnTameFailChance.ToStringPercentEmptyZero("F2"))));
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
			return list.AverageWeighted((Func<VerbProperties, float>)((VerbProperties v) => v.BaseSelectionWeight), (Func<VerbProperties, float>)((VerbProperties v) => (float)v.meleeDamageBaseAmount / (v.defaultCooldownTime + v.warmupTime)));
		}

		public static void DoTable_RacesFoodConsumption()
		{
			Func<ThingDef, int, string> lsName = (Func<ThingDef, int, string>)delegate(ThingDef d, int lsIndex)
			{
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					return string.Empty;
				}
				LifeStageDef def3 = d.race.lifeStageAges[lsIndex].def;
				return def3.defName;
			};
			Func<ThingDef, int, string> maxFood = (Func<ThingDef, int, string>)delegate(ThingDef d, int lsIndex)
			{
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					return string.Empty;
				}
				LifeStageDef def2 = d.race.lifeStageAges[lsIndex].def;
				return (d.race.baseBodySize * def2.bodySizeFactor * def2.foodMaxFactor).ToString("F2");
			};
			Func<ThingDef, int, string> hungerRate = (Func<ThingDef, int, string>)delegate(ThingDef d, int lsIndex)
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
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("Lifestage 0", (Func<ThingDef, string>)((ThingDef d) => lsName(d, 0))), new TableDataGetter<ThingDef>("maxFood", (Func<ThingDef, string>)((ThingDef d) => maxFood(d, 0))), new TableDataGetter<ThingDef>("hungerRate", (Func<ThingDef, string>)((ThingDef d) => hungerRate(d, 0))), new TableDataGetter<ThingDef>("Lifestage 1", (Func<ThingDef, string>)((ThingDef d) => lsName(d, 1))), new TableDataGetter<ThingDef>("maxFood", (Func<ThingDef, string>)((ThingDef d) => maxFood(d, 1))), new TableDataGetter<ThingDef>("hungerRate", (Func<ThingDef, string>)((ThingDef d) => hungerRate(d, 1))), new TableDataGetter<ThingDef>("Lifestage 2", (Func<ThingDef, string>)((ThingDef d) => lsName(d, 2))), new TableDataGetter<ThingDef>("maxFood", (Func<ThingDef, string>)((ThingDef d) => maxFood(d, 2))), new TableDataGetter<ThingDef>("hungerRate", (Func<ThingDef, string>)((ThingDef d) => hungerRate(d, 2))), new TableDataGetter<ThingDef>("Lifestage 3", (Func<ThingDef, string>)((ThingDef d) => lsName(d, 3))), new TableDataGetter<ThingDef>("maxFood", (Func<ThingDef, string>)((ThingDef d) => maxFood(d, 3))), new TableDataGetter<ThingDef>("hungerRate", (Func<ThingDef, string>)((ThingDef d) => hungerRate(d, 3))));
		}

		public static void DoTable_PlantsBasics()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.plant.fertilitySensitivity
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("growDays", (Func<ThingDef, string>)((ThingDef d) => d.plant.growDays.ToString("F2"))), new TableDataGetter<ThingDef>("reproduceMtb", (Func<ThingDef, string>)((ThingDef d) => d.plant.reproduceMtbDays.ToString("F2"))), new TableDataGetter<ThingDef>("nutrition", (Func<ThingDef, string>)((ThingDef d) => (d.ingestible == null) ? "-" : d.ingestible.nutrition.ToString("F2"))), new TableDataGetter<ThingDef>("nut/day", (Func<ThingDef, string>)((ThingDef d) => (d.ingestible == null) ? "-" : (d.ingestible.nutrition / d.plant.growDays).ToString("F4"))), new TableDataGetter<ThingDef>("fertilityMin", (Func<ThingDef, string>)((ThingDef d) => d.plant.fertilityMin.ToString("F2"))), new TableDataGetter<ThingDef>("fertilitySensitivity", (Func<ThingDef, string>)((ThingDef d) => d.plant.fertilitySensitivity.ToString("F2"))));
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
			Func<ThingDef, int> damageGetter = (Func<ThingDef, int>)((ThingDef d) => (d.Verbs[0].projectileDef != null) ? d.Verbs[0].projectileDef.projectile.damageAmountBase : 0);
			Func<ThingDef, float> warmupGetter = (Func<ThingDef, float>)((ThingDef d) => d.Verbs[0].warmupTime);
			Func<ThingDef, float> cooldownGetter = (Func<ThingDef, float>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, null));
			Func<ThingDef, int> burstShotsGetter = (Func<ThingDef, int>)((ThingDef d) => d.Verbs[0].burstShotCount);
			Func<ThingDef, float> dpsRawGetter = (Func<ThingDef, float>)delegate(ThingDef d)
			{
				int num2 = burstShotsGetter(d);
				float num3 = warmupGetter(d) + cooldownGetter(d);
				num3 = (float)(num3 + (float)(num2 - 1) * ((float)d.Verbs[0].ticksBetweenBurstShots / 60.0));
				return (float)(damageGetter(d) * num2) / num3;
			};
			Func<ThingDef, float> accTouchGetter = (Func<ThingDef, float>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyTouch, null));
			Func<ThingDef, float> accShortGetter = (Func<ThingDef, float>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyShort, null));
			Func<ThingDef, float> accMedGetter = (Func<ThingDef, float>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyMedium, null));
			Func<ThingDef, float> accLongGetter = (Func<ThingDef, float>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyLong, null));
			Func<ThingDef, float> dpsAvgGetter = (Func<ThingDef, float>)delegate(ThingDef d)
			{
				float num = 0f;
				num += dpsRawGetter(d) * accShortGetter(d);
				num += dpsRawGetter(d) * accMedGetter(d);
				num += dpsRawGetter(d) * accLongGetter(d);
				return (float)(num / 3.0);
			};
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.IsRangedWeapon
			orderby d.GetStatValueAbstract(StatDefOf.MarketValue, null)
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("damage", (Func<ThingDef, string>)((ThingDef d) => damageGetter(d).ToString())), new TableDataGetter<ThingDef>("warmup", (Func<ThingDef, string>)((ThingDef d) => warmupGetter(d).ToString("F2"))), new TableDataGetter<ThingDef>("burst", (Func<ThingDef, string>)((ThingDef d) => burstShotsGetter(d).ToString())), new TableDataGetter<ThingDef>("cooldown", (Func<ThingDef, string>)((ThingDef d) => cooldownGetter(d).ToString("F2"))), new TableDataGetter<ThingDef>("dpsRaw", (Func<ThingDef, string>)((ThingDef d) => dpsRawGetter(d).ToString("F3"))), new TableDataGetter<ThingDef>("accTouch", (Func<ThingDef, string>)((ThingDef d) => accTouchGetter(d).ToStringPercent())), new TableDataGetter<ThingDef>("accShort", (Func<ThingDef, string>)((ThingDef d) => accShortGetter(d).ToStringPercent())), new TableDataGetter<ThingDef>("accMed", (Func<ThingDef, string>)((ThingDef d) => accMedGetter(d).ToStringPercent())), new TableDataGetter<ThingDef>("accLong", (Func<ThingDef, string>)((ThingDef d) => accLongGetter(d).ToStringPercent())), new TableDataGetter<ThingDef>("dpsTouch", (Func<ThingDef, string>)((ThingDef d) => (dpsRawGetter(d) * accTouchGetter(d)).ToString("F2"))), new TableDataGetter<ThingDef>("dpsShort", (Func<ThingDef, string>)((ThingDef d) => (dpsRawGetter(d) * accShortGetter(d)).ToString("F2"))), new TableDataGetter<ThingDef>("dpsMed", (Func<ThingDef, string>)((ThingDef d) => (dpsRawGetter(d) * accMedGetter(d)).ToString("F2"))), new TableDataGetter<ThingDef>("dpsLong", (Func<ThingDef, string>)((ThingDef d) => (dpsRawGetter(d) * accLongGetter(d)).ToString("F2"))), new TableDataGetter<ThingDef>("dpsAvg", (Func<ThingDef, string>)((ThingDef d) => dpsAvgGetter(d).ToString("F2"))), new TableDataGetter<ThingDef>("mktval", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"))), new TableDataGetter<ThingDef>("work", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, null).ToString("F0"))));
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
			Func<Def, float> damageGetter = (Func<Def, float>)delegate(Def d)
			{
				ThingDef thingDef5 = d as ThingDef;
				if (thingDef5 != null)
				{
					if (thingDef5.race != null)
					{
						return thingDef5.Verbs.AverageWeighted((Func<VerbProperties, float>)((VerbProperties v) => v.BaseSelectionWeight), (Func<VerbProperties, float>)((VerbProperties v) => (float)v.meleeDamageBaseAmount));
					}
					return thingDef5.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageAmount, stuff);
				}
				HediffDef hediffDef4 = d as HediffDef;
				if (hediffDef4 != null)
				{
					return (float)hediffDef4.CompProps<HediffCompProperties_VerbGiver>().verbs[0].meleeDamageBaseAmount;
				}
				return -1f;
			};
			Func<Def, float> warmupGetter = (Func<Def, float>)delegate(Def d)
			{
				ThingDef thingDef4 = d as ThingDef;
				if (thingDef4 != null)
				{
					return thingDef4.Verbs.AverageWeighted((Func<VerbProperties, float>)((VerbProperties v) => v.BaseSelectionWeight), (Func<VerbProperties, float>)((VerbProperties v) => v.warmupTime));
				}
				HediffDef hediffDef3 = d as HediffDef;
				if (hediffDef3 != null)
				{
					return hediffDef3.CompProps<HediffCompProperties_VerbGiver>().verbs[0].warmupTime;
				}
				return -1f;
			};
			Func<Def, float> cooldownGetter = (Func<Def, float>)delegate(Def d)
			{
				ThingDef thingDef3 = d as ThingDef;
				if (thingDef3 != null)
				{
					if (thingDef3.race != null)
					{
						return thingDef3.Verbs.AverageWeighted((Func<VerbProperties, float>)((VerbProperties v) => v.BaseSelectionWeight), (Func<VerbProperties, float>)((VerbProperties v) => v.defaultCooldownTime));
					}
					return thingDef3.GetStatValueAbstract(StatDefOf.MeleeWeapon_Cooldown, stuff);
				}
				HediffDef hediffDef2 = d as HediffDef;
				if (hediffDef2 != null)
				{
					return hediffDef2.CompProps<HediffCompProperties_VerbGiver>().verbs[0].defaultCooldownTime;
				}
				return -1f;
			};
			Func<Def, float> dpsGetter = (Func<Def, float>)((Def d) => damageGetter(d) / (warmupGetter(d) + cooldownGetter(d)));
			Func<Def, float> marketValueGetter = (Func<Def, float>)delegate(Def d)
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
			DebugTables.MakeTablesDialog(enumerable, new TableDataGetter<Def>("defName", (Func<Def, string>)((Def d) => d.defName)), new TableDataGetter<Def>("damage", (Func<Def, string>)((Def d) => damageGetter(d).ToString())), new TableDataGetter<Def>("warmup", (Func<Def, string>)((Def d) => warmupGetter(d).ToString("F2"))), new TableDataGetter<Def>("cooldown", (Func<Def, string>)((Def d) => cooldownGetter(d).ToString("F2"))), new TableDataGetter<Def>("dps", (Func<Def, string>)((Def d) => dpsGetter(d).ToString("F2"))), new TableDataGetter<Def>("mktval", (Func<Def, string>)((Def d) => marketValueGetter(d).ToString("F0"))), new TableDataGetter<Def>("work", (Func<Def, string>)delegate(Def d)
			{
				ThingDef thingDef = d as ThingDef;
				if (thingDef == null)
				{
					return "-";
				}
				return thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0");
			}));
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
						List<Apparel>.Enumerator enumerator2 = pawn.apparel.WornApparel.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								Apparel current2 = enumerator2.Current;
								DefMap<ThingDef, int> defMap;
								DefMap<ThingDef, int> obj = defMap = appCounts;
								ThingDef def;
								ThingDef def2 = def = current2.def;
								int num = defMap[def];
								obj[def2] = num + 1;
							}
						}
						finally
						{
							((IDisposable)(object)enumerator2).Dispose();
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
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("bodyParts", (Func<ThingDef, string>)((ThingDef d) => GenText.ToSpaceList(d.apparel.bodyPartGroups.Select((Func<BodyPartGroupDef, string>)((BodyPartGroupDef bp) => bp.defName))))), new TableDataGetter<ThingDef>("layers", (Func<ThingDef, string>)((ThingDef d) => GenText.ToSpaceList(d.apparel.layers.Select((Func<ApparelLayer, string>)((ApparelLayer l) => ((Enum)(object)l).ToString()))))), new TableDataGetter<ThingDef>("tags", (Func<ThingDef, string>)((ThingDef d) => GenText.ToSpaceList(d.apparel.tags.Select((Func<string, string>)((string t) => t.ToString()))))), new TableDataGetter<ThingDef>("insCold", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff).ToString("F0"))), new TableDataGetter<ThingDef>("insHeat", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff).ToString("F0"))), new TableDataGetter<ThingDef>("mktval", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, stuff).ToString("F0"))), new TableDataGetter<ThingDef>("work", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0"))));
		}

		public static void DoTable_HitPoints()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.useHitPoints
			orderby d.GetStatValueAbstract(StatDefOf.MaxHitPoints, null) descending
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("hp", (Func<ThingDef, string>)((ThingDef d) => d.BaseMaxHitPoints.ToString())), new TableDataGetter<ThingDef>("category", (Func<ThingDef, string>)((ThingDef d) => ((Enum)(object)d.category).ToString())));
		}

		public static void DoTable_FillPercent()
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where d.fillPercent > 0.0
			orderby d.fillPercent descending
			select d, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)((ThingDef d) => d.defName)), new TableDataGetter<ThingDef>("fillPercent", (Func<ThingDef, string>)((ThingDef d) => d.fillPercent.ToStringPercent())), new TableDataGetter<ThingDef>("category", (Func<ThingDef, string>)((ThingDef d) => ((Enum)(object)d.category).ToString())));
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
				List<SkillNeed>.Enumerator enumerator = stat.skillNeedFactors.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						SkillNeed current = enumerator.Current;
						SkillNeed_Direct skillNeed_Direct = current as SkillNeed_Direct;
						num *= skillNeed_Direct.factorsPerLevel[level];
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
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
					select d, new TableDataGetter<BodyPartRecord>("defName", (Func<BodyPartRecord, string>)((BodyPartRecord d) => d.def.defName)), new TableDataGetter<BodyPartRecord>("coverage", (Func<BodyPartRecord, string>)((BodyPartRecord d) => d.coverage.ToStringPercent())), new TableDataGetter<BodyPartRecord>("coverageAbsWithChildren", (Func<BodyPartRecord, string>)((BodyPartRecord d) => d.coverageAbsWithChildren.ToStringPercent())), new TableDataGetter<BodyPartRecord>("coverageAbs", (Func<BodyPartRecord, string>)((BodyPartRecord d) => d.coverageAbs.ToStringPercent())), new TableDataGetter<BodyPartRecord>("depth", (Func<BodyPartRecord, string>)((BodyPartRecord d) => ((Enum)(object)d.depth).ToString())), new TableDataGetter<BodyPartRecord>("height", (Func<BodyPartRecord, string>)((BodyPartRecord d) => ((Enum)(object)d.height).ToString())));
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
				if (num3 == 0.0)
				{
					return string.Empty;
				}
				return num3.ToStringPercent("F1");
			})).ToList();
			list.Insert(0, new TableDataGetter<PawnKindDef>("animal", (Func<PawnKindDef, string>)((PawnKindDef k) => k.defName + ((!k.race.race.predator) ? string.Empty : "*"))));
			DebugTables.MakeTablesDialog(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.defName
			select d, list.ToArray());
		}

		public static void DoTable_ThingMasses()
		{
			IOrderedEnumerable<ThingDef> dataSources = from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Item || x.Minifiable
			where x.thingClass != typeof(MinifiedThing) && x.thingClass != typeof(UnfinishedThing)
			orderby x.GetStatValueAbstract(StatDefOf.Mass, null), x.GetStatValueAbstract(StatDefOf.MarketValue, null)
			select x;
			Func<ThingDef, float, string> perPawn = (Func<ThingDef, float, string>)((ThingDef d, float bodySize) => ((float)(bodySize * 35.0 / d.GetStatValueAbstract(StatDefOf.Mass, null))).ToString("F0"));
			Func<ThingDef, string> perNutrition = (Func<ThingDef, string>)delegate(ThingDef d)
			{
				if (d.ingestible != null && d.ingestible.nutrition != 0.0)
				{
					return (d.GetStatValueAbstract(StatDefOf.Mass, null) / d.ingestible.nutrition).ToString("F2");
				}
				return string.Empty;
			};
			DebugTables.MakeTablesDialog(dataSources, new TableDataGetter<ThingDef>("defName", (Func<ThingDef, string>)delegate(ThingDef d)
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
			}), new TableDataGetter<ThingDef>("mass", (Func<ThingDef, string>)((ThingDef d) => d.GetStatValueAbstract(StatDefOf.Mass, null).ToString())), new TableDataGetter<ThingDef>("per human", (Func<ThingDef, string>)((ThingDef d) => perPawn(d, ThingDefOf.Human.race.baseBodySize))), new TableDataGetter<ThingDef>("per muffalo", (Func<ThingDef, string>)((ThingDef d) => perPawn(d, ThingDefOf.Muffalo.race.baseBodySize))), new TableDataGetter<ThingDef>("per dromedary", (Func<ThingDef, string>)((ThingDef d) => perPawn(d, ThingDefOf.Dromedary.race.baseBodySize))), new TableDataGetter<ThingDef>("per nutrition", (Func<ThingDef, string>)((ThingDef d) => perNutrition(d))), new TableDataGetter<ThingDef>("small volume", (Func<ThingDef, string>)((ThingDef d) => (!d.smallVolume) ? string.Empty : "small")));
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
				float factor = skillNeed_Direct.factorsPerLevel[i];
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
				list.Add(new TableDataGetter<ThingDef>(defName, (Func<ThingDef, string>)((ThingDef td) => (!localTk.WillTrade(td)) ? string.Empty : "✓")));
			}
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.0010000000474974513 && !d.isUnfinishedThing && !d.IsCorpse && !d.destroyOnDrop && d != ThingDefOf.Silver && !d.thingCategories.NullOrEmpty()) || (d.category == ThingCategory.Building && d.Minifiable)
			orderby d.thingCategories.NullOrEmpty() ? "zzzzzzz" : d.thingCategories[0].defName, d.BaseMarketValue
			select d, list.ToArray());
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
	}
}
