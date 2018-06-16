using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E1A RID: 3610
	[HasDebugOutput]
	internal static class DebugOutputsEconomy
	{
		// Token: 0x060051FF RID: 20991 RVA: 0x0029F20C File Offset: 0x0029D60C
		[DebugOutput]
		[Category("Economy")]
		public static void RecipeSkills()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Recipes per skill, with work speed stats:");
			stringBuilder.AppendLine("(No skill)");
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
			{
				if (recipeDef.workSkill == null)
				{
					stringBuilder.Append("    " + recipeDef.defName);
					if (recipeDef.workSpeedStat != null)
					{
						stringBuilder.Append(" (" + recipeDef.workSpeedStat + ")");
					}
					stringBuilder.AppendLine();
				}
			}
			stringBuilder.AppendLine();
			foreach (SkillDef skillDef in DefDatabase<SkillDef>.AllDefs)
			{
				stringBuilder.AppendLine(skillDef.LabelCap);
				foreach (RecipeDef recipeDef2 in DefDatabase<RecipeDef>.AllDefs)
				{
					if (recipeDef2.workSkill == skillDef)
					{
						stringBuilder.Append("    " + recipeDef2.defName);
						if (recipeDef2.workSpeedStat != null)
						{
							stringBuilder.Append(" (" + recipeDef2.workSpeedStat);
							if (!recipeDef2.workSpeedStat.skillNeedFactors.NullOrEmpty<SkillNeed>())
							{
								stringBuilder.Append(" - " + (from fac in recipeDef2.workSpeedStat.skillNeedFactors
								select fac.skill.defName).ToCommaList(false));
							}
							stringBuilder.Append(")");
						}
						stringBuilder.AppendLine();
					}
				}
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString().TrimEndNewlines(), false);
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x0029F470 File Offset: 0x0029D870
		[DebugOutput]
		[Category("Economy")]
		public static void Drugs()
		{
			Func<ThingDef, float> realIngredientCost = (ThingDef d) => DebugOutputsEconomy.CostToMake(d, true);
			Func<ThingDef, float> realSellPrice = (ThingDef d) => d.BaseMarketValue * 0.5f;
			Func<ThingDef, float> realBuyPrice = (ThingDef d) => d.BaseMarketValue * 1.5f;
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsWithinCategory(ThingCategoryDefOf.Medicine) || d.IsWithinCategory(ThingCategoryDefOf.Drugs)
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[8];
			array[0] = new TableDataGetter<ThingDef>("name", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("ingredients", (ThingDef d) => DebugOutputsEconomy.CostListString(d, true, true));
			array[2] = new TableDataGetter<ThingDef>("work amount", (ThingDef d) => DebugOutputsEconomy.WorkToProduceBest(d).ToString("F0"));
			array[3] = new TableDataGetter<ThingDef>("real ingredient cost", (ThingDef d) => realIngredientCost(d).ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("real sell price", (ThingDef d) => realSellPrice(d).ToString("F1"));
			array[5] = new TableDataGetter<ThingDef>("real profit per item", (ThingDef d) => (realSellPrice(d) - realIngredientCost(d)).ToString("F1"));
			array[6] = new TableDataGetter<ThingDef>("real profit per day's work", (ThingDef d) => ((realSellPrice(d) - realIngredientCost(d)) / DebugOutputsEconomy.WorkToProduceBest(d) * 30000f).ToString("F1"));
			array[7] = new TableDataGetter<ThingDef>("real buy price", (ThingDef d) => realBuyPrice(d).ToString("F1"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x0029F61C File Offset: 0x0029DA1C
		[DebugOutput]
		[Category("Economy")]
		public static void Wool()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh && d.GetCompProperties<CompProperties_Shearable>() != null
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("animal", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("woolDef", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolDef.defName);
			array[2] = new TableDataGetter<ThingDef>("woolAmount", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolAmount.ToString());
			array[3] = new TableDataGetter<ThingDef>("woolValue", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolDef.BaseMarketValue.ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("shear interval", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().shearIntervalDays.ToString("F1"));
			array[5] = new TableDataGetter<ThingDef>("value per year", delegate(ThingDef d)
			{
				CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
				return (compProperties.woolDef.BaseMarketValue * (float)compProperties.woolAmount * (60f / (float)compProperties.shearIntervalDays)).ToString("F0");
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x0029F758 File Offset: 0x0029DB58
		[DebugOutput]
		[Category("Economy")]
		public static void AnimalGrowth()
		{
			Func<ThingDef, float> gestDaysEach = (ThingDef d) => DebugOutputsEconomy.GestationDaysEach(d);
			Func<ThingDef, float> nutritionToGestate = delegate(ThingDef d)
			{
				float num = 0f;
				LifeStageAge lifeStageAge = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1];
				return num + gestDaysEach(d) * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
			};
			Func<ThingDef, float> babyMeatNut = delegate(ThingDef d)
			{
				LifeStageAge lifeStageAge = d.race.lifeStageAges[0];
				return d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f * lifeStageAge.def.bodySizeFactor;
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
					float num3 = num2 * 60f;
					num += num3 * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
				}
				return num;
			};
			Func<ThingDef, float> adultMeatNutPerInput = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f / nutritionToAdulthood(d);
			Func<ThingDef, float> bestMeatPerInput = delegate(ThingDef d)
			{
				float a = babyMeatNutPerInput(d);
				float b = adultMeatNutPerInput(d);
				return Mathf.Max(a, b);
			};
			Func<ThingDef, string> eggNut = delegate(ThingDef d)
			{
				CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				string result;
				if (compProperties == null)
				{
					result = "";
				}
				else
				{
					result = compProperties.eggFertilizedDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2");
				}
				return result;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh
			orderby bestMeatPerInput(d) descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[17];
			array[0] = new TableDataGetter<ThingDef>("", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => d.race.baseHungerRate.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("gestDaysEach", (ThingDef d) => gestDaysEach(d).ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("herbiv", (ThingDef d) => ((d.race.foodType & FoodTypeFlags.Plant) == FoodTypeFlags.None) ? "" : "Y");
			array[4] = new TableDataGetter<ThingDef>("|", (ThingDef d) => "|");
			array[5] = new TableDataGetter<ThingDef>("bodySize", (ThingDef d) => d.race.baseBodySize.ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("age Adult", (ThingDef d) => d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge.ToString("F2"));
			array[7] = new TableDataGetter<ThingDef>("nutrition to adulthood", (ThingDef d) => nutritionToAdulthood(d).ToString("F2"));
			array[8] = new TableDataGetter<ThingDef>("adult meat-nut", (ThingDef d) => (d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("adult meat-nut / input-nut", (ThingDef d) => adultMeatNutPerInput(d).ToString("F3"));
			array[10] = new TableDataGetter<ThingDef>("|", (ThingDef d) => "|");
			array[11] = new TableDataGetter<ThingDef>("baby size", (ThingDef d) => (d.race.lifeStageAges[0].def.bodySizeFactor * d.race.baseBodySize).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("nutrition to gestate", (ThingDef d) => nutritionToGestate(d).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("egg nut", (ThingDef d) => eggNut(d));
			array[14] = new TableDataGetter<ThingDef>("baby meat-nut", (ThingDef d) => babyMeatNut(d).ToString("F2"));
			array[15] = new TableDataGetter<ThingDef>("baby meat-nut / input-nut", (ThingDef d) => babyMeatNutPerInput(d).ToString("F2"));
			array[16] = new TableDataGetter<ThingDef>("baby wins", (ThingDef d) => (babyMeatNutPerInput(d) <= adultMeatNutPerInput(d)) ? "" : "B");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x0029FAC0 File Offset: 0x0029DEC0
		[DebugOutput]
		[Category("Economy")]
		public static void AnimalBreeding()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh
			orderby DebugOutputsEconomy.GestationDaysEach(d) descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("gestDaysEach", (ThingDef d) => DebugOutputsEconomy.GestationDaysEach(d).ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("avgOffspring", (ThingDef d) => (!d.HasComp(typeof(CompEggLayer))) ? ((d.race.litterSizeCurve == null) ? 1f : Rand.ByCurveAverage(d.race.litterSizeCurve)).ToString("F2") : d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average.ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("gestDaysRaw", (ThingDef d) => (!d.HasComp(typeof(CompEggLayer))) ? d.race.gestationPeriodDays.ToString("F1") : d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays.ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("growth per 30d", delegate(ThingDef d)
			{
				float f = 1f + ((!d.HasComp(typeof(CompEggLayer))) ? ((d.race.litterSizeCurve == null) ? 1f : Rand.ByCurveAverage(d.race.litterSizeCurve)) : d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average);
				float num = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge * 60f;
				float num2 = num + ((!d.HasComp(typeof(CompEggLayer))) ? d.race.gestationPeriodDays : d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays);
				float p = 30f / num2;
				return Mathf.Pow(f, p).ToString("F2");
			});
			array[5] = new TableDataGetter<ThingDef>("growth per 60d", delegate(ThingDef d)
			{
				float f = 1f + ((!d.HasComp(typeof(CompEggLayer))) ? ((d.race.litterSizeCurve == null) ? 1f : Rand.ByCurveAverage(d.race.litterSizeCurve)) : d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average);
				float num = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge * 60f;
				float num2 = num + ((!d.HasComp(typeof(CompEggLayer))) ? d.race.gestationPeriodDays : d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays);
				float p = 60f / num2;
				return Mathf.Pow(f, p).ToString("F2");
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005204 RID: 20996 RVA: 0x0029FC20 File Offset: 0x0029E020
		private static float GestationDaysEach(ThingDef d)
		{
			float result;
			if (d.HasComp(typeof(CompEggLayer)))
			{
				CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				result = compProperties.eggLayIntervalDays / compProperties.eggCountRange.Average;
			}
			else
			{
				result = d.race.gestationPeriodDays / ((d.race.litterSizeCurve == null) ? 1f : Rand.ByCurveAverage(d.race.litterSizeCurve));
			}
			return result;
		}

		// Token: 0x06005205 RID: 20997 RVA: 0x0029FCA4 File Offset: 0x0029E0A4
		[DebugOutput]
		[Category("Economy")]
		public static void BuildingSkills()
		{
			IEnumerable<BuildableDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>())
			where d.BuildableByPlayer
			select d;
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[2];
			array[0] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[1] = new TableDataGetter<BuildableDef>("construction skill prerequisite", (BuildableDef d) => d.constructionSkillPrerequisite);
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x0029FD4C File Offset: 0x0029E14C
		[DebugOutput]
		[Category("Economy")]
		public static void Crops()
		{
			Func<ThingDef, float> workCost = delegate(ThingDef d)
			{
				float num = 1.1f;
				num += d.plant.growDays * 1f;
				return num + (d.plant.sowWork + d.plant.harvestWork) * 0.00612f;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant && d.plant.Harvestable && d.plant.Sowable
			orderby d.plant.IsTree
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[14];
			array[0] = new TableDataGetter<ThingDef>("plant", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("product", (ThingDef d) => d.plant.harvestedThingDef.defName);
			array[2] = new TableDataGetter<ThingDef>("grow time", (ThingDef d) => d.plant.growDays.ToString("F1"));
			array[3] = new TableDataGetter<ThingDef>("work", (ThingDef d) => (d.plant.sowWork + d.plant.harvestWork).ToString("F0"));
			array[4] = new TableDataGetter<ThingDef>("harvestCount", (ThingDef d) => d.plant.harvestYield.ToString("F1"));
			array[5] = new TableDataGetter<ThingDef>("work-cost per cycle", (ThingDef d) => workCost(d).ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("work-cost per harvestCount", (ThingDef d) => (workCost(d) / d.plant.harvestYield).ToString("F2"));
			array[7] = new TableDataGetter<ThingDef>("value each", (ThingDef d) => d.plant.harvestedThingDef.BaseMarketValue.ToString("F2"));
			array[8] = new TableDataGetter<ThingDef>("harvestValueTotal", (ThingDef d) => (d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("profit per growDay", (ThingDef d) => ((d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue - workCost(d)) / d.plant.growDays).ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("nutrition per growDay", (ThingDef d) => (d.plant.harvestedThingDef.ingestible == null) ? "" : (d.plant.harvestYield * d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.plant.growDays).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => (d.plant.harvestedThingDef.ingestible == null) ? "" : d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("fertMin", (ThingDef d) => d.plant.fertilityMin.ToStringPercent());
			array[13] = new TableDataGetter<ThingDef>("fertSensitivity", (ThingDef d) => d.plant.fertilitySensitivity.ToStringPercent());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005207 RID: 20999 RVA: 0x0029FFF8 File Offset: 0x0029E3F8
		[DebugOutput]
		[Category("Economy")]
		public static void ItemAndBuildingAcquisition()
		{
			Func<ThingDef, string> recipes = delegate(ThingDef d)
			{
				List<string> list = new List<string>();
				foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
				{
					if (!recipeDef.products.NullOrEmpty<ThingDefCountClass>())
					{
						for (int i = 0; i < recipeDef.products.Count; i++)
						{
							if (recipeDef.products[i].thingDef == d)
							{
								list.Add(recipeDef.defName);
							}
						}
					}
				}
				string result;
				if (list.Count == 0)
				{
					result = "";
				}
				else
				{
					result = list.ToCommaList(false);
				}
				return result;
			};
			Func<ThingDef, string> workAmountSources = delegate(ThingDef d)
			{
				List<string> list = new List<string>();
				if (d.StatBaseDefined(StatDefOf.WorkToMake))
				{
					list.Add("WorkToMake(" + d.GetStatValueAbstract(StatDefOf.WorkToMake, null) + ")");
				}
				if (d.StatBaseDefined(StatDefOf.WorkToBuild))
				{
					list.Add("WorkToBuild(" + d.GetStatValueAbstract(StatDefOf.WorkToBuild, null) + ")");
				}
				foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
				{
					if (recipeDef.workAmount > 0f && !recipeDef.products.NullOrEmpty<ThingDefCountClass>())
					{
						for (int i = 0; i < recipeDef.products.Count; i++)
						{
							if (recipeDef.products[i].thingDef == d)
							{
								list.Add(string.Concat(new object[]
								{
									"RecipeDef-",
									recipeDef.defName,
									"(",
									recipeDef.workAmount,
									")"
								}));
							}
						}
					}
				}
				string result;
				if (list.Count == 0)
				{
					result = "";
				}
				else
				{
					result = list.ToCommaList(false);
				}
				return result;
			};
			Func<ThingDef, string> calculatedMarketValue = delegate(ThingDef d)
			{
				string result;
				if (!DebugOutputsEconomy.Producible(d))
				{
					result = "not producible";
				}
				else if (!d.StatBaseDefined(StatDefOf.MarketValue))
				{
					result = "used";
				}
				else
				{
					string text = StatWorker_MarketValue.CalculatedMarketValue(d, null).ToString("F1");
					if (StatWorker_MarketValue.CalculableRecipe(d) != null)
					{
						result = text + " (recipe)";
					}
					else
					{
						result = text;
					}
				}
				return result;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.01f) || (d.category == ThingCategory.Building && (d.BuildableByPlayer || d.Minifiable))
			orderby d.BaseMarketValue
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[16];
			array[0] = new TableDataGetter<ThingDef>("cat.", (ThingDef d) => d.category.ToString().Substring(0, 1).CapitalizeFirst());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("mobile", (ThingDef d) => (d.category == ThingCategory.Item || d.Minifiable).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("base\nmarket value", (ThingDef d) => d.BaseMarketValue.ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("calculated\nmarket value", (ThingDef d) => calculatedMarketValue(d));
			array[5] = new TableDataGetter<ThingDef>("cost to make", (ThingDef d) => DebugOutputsEconomy.CostToMakeString(d, false));
			array[6] = new TableDataGetter<ThingDef>("work to produce", (ThingDef d) => (DebugOutputsEconomy.WorkToProduceBest(d) <= 0f) ? "-" : DebugOutputsEconomy.WorkToProduceBest(d).ToString("F1"));
			array[7] = new TableDataGetter<ThingDef>("profit", (ThingDef d) => (d.BaseMarketValue - DebugOutputsEconomy.CostToMake(d, false)).ToString("F1"));
			array[8] = new TableDataGetter<ThingDef>("profit\nrate", (ThingDef d) => (d.recipeMaker == null) ? "-" : ((d.BaseMarketValue - DebugOutputsEconomy.CostToMake(d, false)) / DebugOutputsEconomy.WorkToProduceBest(d) * 10000f).ToString("F0"));
			array[9] = new TableDataGetter<ThingDef>("market value\ndefined", (ThingDef d) => d.statBases.Any((StatModifier st) => st.stat == StatDefOf.MarketValue).ToStringCheckBlank());
			array[10] = new TableDataGetter<ThingDef>("producible", (ThingDef d) => DebugOutputsEconomy.Producible(d).ToStringCheckBlank());
			array[11] = new TableDataGetter<ThingDef>("thing set\nmaker tags", (ThingDef d) => (!d.thingSetMakerTags.NullOrEmpty<string>()) ? d.thingSetMakerTags.ToCommaList(false) : "");
			array[12] = new TableDataGetter<ThingDef>("made\nfrom\nstuff", (ThingDef d) => d.MadeFromStuff.ToStringCheckBlank());
			array[13] = new TableDataGetter<ThingDef>("cost list", (ThingDef d) => DebugOutputsEconomy.CostListString(d, false, false));
			array[14] = new TableDataGetter<ThingDef>("recipes", (ThingDef d) => recipes(d));
			array[15] = new TableDataGetter<ThingDef>("work amount\nsources", (ThingDef d) => workAmountSources(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005208 RID: 21000 RVA: 0x002A0340 File Offset: 0x0029E740
		[DebugOutput]
		[Category("Economy")]
		public static void ItemAccessibility()
		{
			IEnumerable<ThingDef> dataSources = from x in ThingSetMakerUtility.allGeneratableItems
			orderby x.defName
			select x;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("1", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1, Find.CurrentMap)) ? "" : "✓");
			array[2] = new TableDataGetter<ThingDef>("10", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10, Find.CurrentMap)) ? "" : "✓");
			array[3] = new TableDataGetter<ThingDef>("100", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 100, Find.CurrentMap)) ? "" : "✓");
			array[4] = new TableDataGetter<ThingDef>("1000", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1000, Find.CurrentMap)) ? "" : "✓");
			array[5] = new TableDataGetter<ThingDef>("10000", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10000, Find.CurrentMap)) ? "" : "✓");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005209 RID: 21001 RVA: 0x002A047C File Offset: 0x0029E87C
		[DebugOutput]
		[Category("Economy")]
		public static void ThingSmeltProducts()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				Thing thing = ThingMaker.MakeThing(thingDef, GenStuff.DefaultStuffFor(thingDef));
				if (thing.SmeltProducts(1f).Any<Thing>())
				{
					stringBuilder.Append(thing.LabelCap + ": ");
					foreach (Thing thing2 in thing.SmeltProducts(1f))
					{
						stringBuilder.Append(" " + thing2.Label);
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600520A RID: 21002 RVA: 0x002A058C File Offset: 0x0029E98C
		[DebugOutput]
		[Category("Economy")]
		public static void Recipes()
		{
			IEnumerable<RecipeDef> dataSources = from d in DefDatabase<RecipeDef>.AllDefs
			where !d.products.NullOrEmpty<ThingDefCountClass>() && !d.ingredients.NullOrEmpty<IngredientCount>()
			select d;
			TableDataGetter<RecipeDef>[] array = new TableDataGetter<RecipeDef>[12];
			array[0] = new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName);
			array[1] = new TableDataGetter<RecipeDef>("work /w carry", (RecipeDef d) => DebugOutputsEconomy.TrueWorkWithCarryTime(d).ToString("F0"));
			array[2] = new TableDataGetter<RecipeDef>("work seconds", (RecipeDef d) => (DebugOutputsEconomy.TrueWorkWithCarryTime(d) / 60f).ToString("F0"));
			array[3] = new TableDataGetter<RecipeDef>("cheapest products value", (RecipeDef d) => DebugOutputsEconomy.CheapestProductsValue(d).ToString("F1"));
			array[4] = new TableDataGetter<RecipeDef>("cheapest ingredients value", (RecipeDef d) => DebugOutputsEconomy.CheapestIngredientValue(d).ToString("F1"));
			array[5] = new TableDataGetter<RecipeDef>("work value", (RecipeDef d) => DebugOutputsEconomy.WorkValueEstimate(d).ToString("F1"));
			array[6] = new TableDataGetter<RecipeDef>("profit raw", (RecipeDef d) => (DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.CheapestIngredientValue(d)).ToString("F1"));
			array[7] = new TableDataGetter<RecipeDef>("profit with work", (RecipeDef d) => (DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.WorkValueEstimate(d) - DebugOutputsEconomy.CheapestIngredientValue(d)).ToString("F1"));
			array[8] = new TableDataGetter<RecipeDef>("profit per work day", (RecipeDef d) => ((DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.CheapestIngredientValue(d)) * 60000f / DebugOutputsEconomy.TrueWorkWithCarryTime(d)).ToString("F0"));
			array[9] = new TableDataGetter<RecipeDef>("min skill", (RecipeDef d) => (!d.skillRequirements.NullOrEmpty<SkillRequirement>()) ? d.skillRequirements[0].Summary : "");
			array[10] = new TableDataGetter<RecipeDef>("cheapest stuff", (RecipeDef d) => (DebugOutputsEconomy.CheapestNonDerpStuff(d) == null) ? "" : DebugOutputsEconomy.CheapestNonDerpStuff(d).defName);
			array[11] = new TableDataGetter<RecipeDef>("cheapest ingredients", (RecipeDef d) => (from pa in DebugOutputsEconomy.CheapestIngredients(d)
			select pa.First.defName + " x" + pa.Second).ToCommaList(false));
			DebugTables.MakeTablesDialog<RecipeDef>(dataSources, array);
		}

		// Token: 0x0600520B RID: 21003 RVA: 0x002A07C8 File Offset: 0x0029EBC8
		private static bool Producible(BuildableDef b)
		{
			ThingDef d = b as ThingDef;
			TerrainDef terrainDef = b as TerrainDef;
			if (d != null)
			{
				if (DefDatabase<RecipeDef>.AllDefs.Any((RecipeDef r) => r.products.Any((ThingDefCountClass pr) => pr.thingDef == d)))
				{
					return true;
				}
				if (d.category == ThingCategory.Building && d.BuildableByPlayer)
				{
					return true;
				}
			}
			else if (terrainDef != null)
			{
				return terrainDef.BuildableByPlayer;
			}
			return false;
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x002A0868 File Offset: 0x0029EC68
		public static string CostListString(BuildableDef d, bool divideByVolume, bool starIfOnlyBuyable)
		{
			string result;
			if (!DebugOutputsEconomy.Producible(d))
			{
				result = "";
			}
			else
			{
				List<string> list = new List<string>();
				if (d.costList != null)
				{
					foreach (ThingDefCountClass thingDefCountClass in d.costList)
					{
						float num = (float)thingDefCountClass.count;
						if (divideByVolume)
						{
							num /= thingDefCountClass.thingDef.VolumePerUnit;
						}
						string text = thingDefCountClass.thingDef + " x" + num;
						if (starIfOnlyBuyable && DebugOutputsEconomy.RequiresBuying(thingDefCountClass.thingDef))
						{
							text += "*";
						}
						list.Add(text);
					}
				}
				if (d.MadeFromStuff)
				{
					list.Add("stuff x" + d.costStuffCount);
				}
				result = list.ToCommaList(false);
			}
			return result;
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x002A0984 File Offset: 0x0029ED84
		private static float TrueWorkWithCarryTime(RecipeDef d)
		{
			ThingDef stuffDef = DebugOutputsEconomy.CheapestNonDerpStuff(d);
			return (float)d.ingredients.Count * 90f + d.WorkAmountTotal(stuffDef) + 90f;
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x002A09C0 File Offset: 0x0029EDC0
		private static float CheapestIngredientValue(RecipeDef d)
		{
			float num = 0f;
			foreach (Pair<ThingDef, float> pair in DebugOutputsEconomy.CheapestIngredients(d))
			{
				num += pair.First.BaseMarketValue * pair.Second;
			}
			return num;
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x002A0A3C File Offset: 0x0029EE3C
		private static IEnumerable<Pair<ThingDef, float>> CheapestIngredients(RecipeDef d)
		{
			foreach (IngredientCount ing in d.ingredients)
			{
				ThingDef thing = (from td in ing.filter.AllowedThingDefs
				where td != ThingDefOf.Meat_Human
				select td).MinBy((ThingDef td) => td.BaseMarketValue / td.VolumePerUnit);
				yield return new Pair<ThingDef, float>(thing, ing.GetBaseCount() / d.IngredientValueGetter.ValuePerUnitOf(thing));
			}
			yield break;
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x002A0A68 File Offset: 0x0029EE68
		private static float WorkValueEstimate(RecipeDef d)
		{
			return DebugOutputsEconomy.TrueWorkWithCarryTime(d) * 0.01f;
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x002A0A8C File Offset: 0x0029EE8C
		private static ThingDef CheapestNonDerpStuff(RecipeDef d)
		{
			ThingDef productDef = d.products[0].thingDef;
			ThingDef result;
			if (!productDef.MadeFromStuff)
			{
				result = null;
			}
			else
			{
				result = (from td in d.ingredients.First<IngredientCount>().filter.AllowedThingDefs
				where !productDef.IsWeapon || !PawnWeaponGenerator.IsDerpWeapon(productDef, td)
				select td).MinBy((ThingDef td) => td.BaseMarketValue / td.VolumePerUnit);
			}
			return result;
		}

		// Token: 0x06005212 RID: 21010 RVA: 0x002A0B20 File Offset: 0x0029EF20
		private static float CheapestProductsValue(RecipeDef d)
		{
			float num = 0f;
			foreach (ThingDefCountClass thingDefCountClass in d.products)
			{
				num += thingDefCountClass.thingDef.GetStatValueAbstract(StatDefOf.MarketValue, DebugOutputsEconomy.CheapestNonDerpStuff(d)) * (float)thingDefCountClass.count;
			}
			return num;
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x002A0BA8 File Offset: 0x0029EFA8
		private static string CostToMakeString(ThingDef d, bool real = false)
		{
			string result;
			if (d.recipeMaker == null)
			{
				result = "-";
			}
			else
			{
				result = DebugOutputsEconomy.CostToMake(d, real).ToString("F1");
			}
			return result;
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x002A0BE8 File Offset: 0x0029EFE8
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
					foreach (ThingDefCountClass thingDefCountClass in d.costList)
					{
						float num2 = 1f;
						if (real)
						{
							num2 = ((!DebugOutputsEconomy.RequiresBuying(thingDefCountClass.thingDef)) ? 0.5f : 1.5f);
						}
						num += (float)thingDefCountClass.count * DebugOutputsEconomy.CostToMake(thingDefCountClass.thingDef, true) * num2;
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

		// Token: 0x06005215 RID: 21013 RVA: 0x002A0CE4 File Offset: 0x0029F0E4
		private static bool RequiresBuying(ThingDef def)
		{
			bool result;
			if (def.costList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in def.costList)
				{
					if (DebugOutputsEconomy.RequiresBuying(thingDefCountClass.thingDef))
					{
						return true;
					}
				}
				result = false;
			}
			else
			{
				result = !DefDatabase<ThingDef>.AllDefs.Any((ThingDef d) => d.plant != null && d.plant.harvestedThingDef == def && d.plant.Sowable);
			}
			return result;
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x002A0DA0 File Offset: 0x0029F1A0
		public static float WorkToProduceBest(BuildableDef d)
		{
			float num = float.MaxValue;
			if (d.StatBaseDefined(StatDefOf.WorkToMake))
			{
				num = d.GetStatValueAbstract(StatDefOf.WorkToMake, null);
			}
			if (d.StatBaseDefined(StatDefOf.WorkToBuild) && d.GetStatValueAbstract(StatDefOf.WorkToBuild, null) < num)
			{
				num = d.GetStatValueAbstract(StatDefOf.WorkToBuild, null);
			}
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
			{
				if (recipeDef.workAmount > 0f && !recipeDef.products.NullOrEmpty<ThingDefCountClass>())
				{
					for (int i = 0; i < recipeDef.products.Count; i++)
					{
						if (recipeDef.products[i].thingDef == d && recipeDef.workAmount < num)
						{
							num = recipeDef.workAmount;
						}
					}
				}
			}
			float result;
			if (num != 3.40282347E+38f)
			{
				result = num;
			}
			else
			{
				result = -1f;
			}
			return result;
		}
	}
}
