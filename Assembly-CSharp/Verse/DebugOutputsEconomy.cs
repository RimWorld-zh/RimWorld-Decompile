using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	[HasDebugOutput]
	public static class DebugOutputsEconomy
	{
		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<RecipeDef, float> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheA;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheB;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cacheC;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheD;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheE;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheF;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache10;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache11;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache12;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache13;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache14;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache15;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache16;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache17;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache18;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache19;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1A;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1B;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1C;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1D;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1E;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__mg$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1F;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache20;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache21;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache22;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache23;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache24;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache25;

		[CompilerGenerated]
		private static Func<BuildableDef, bool> <>f__am$cache26;

		[CompilerGenerated]
		private static Func<BuildableDef, string> <>f__am$cache27;

		[CompilerGenerated]
		private static Func<BuildableDef, int> <>f__am$cache28;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache29;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache2A;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache2B;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2C;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2D;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2E;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2F;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache30;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache31;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache32;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache33;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache34;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache35;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache36;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache37;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache38;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache39;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache3A;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache3B;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3C;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3D;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3E;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3F;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache40;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache41;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache42;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache43;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache44;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache45;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache46;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache47;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache48;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache49;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4A;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4B;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4C;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4D;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4E;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4F;

		[CompilerGenerated]
		private static Func<RecipeDef, bool> <>f__am$cache50;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache51;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache52;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache53;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache54;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache55;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache56;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache57;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache58;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache59;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache5A;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache5B;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache5C;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache5D;

		[CompilerGenerated]
		private static Func<SkillNeed, string> <>f__am$cache5E;

		[CompilerGenerated]
		private static Predicate<StatModifier> <>f__am$cache5F;

		[CompilerGenerated]
		private static Func<Pair<ThingDef, float>, string> <>f__am$cache60;

		[Category("Economy")]
		[DebugOutput]
		public static void RecipeSkills()
		{
			IEnumerable<RecipeDef> allDefs = DefDatabase<RecipeDef>.AllDefs;
			TableDataGetter<RecipeDef>[] array = new TableDataGetter<RecipeDef>[5];
			array[0] = new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName);
			array[1] = new TableDataGetter<RecipeDef>("workSkill", (RecipeDef d) => (d.workSkill != null) ? d.workSkill.defName : string.Empty);
			array[2] = new TableDataGetter<RecipeDef>("workSpeedStat", (RecipeDef d) => (d.workSpeedStat != null) ? d.workSpeedStat.defName : string.Empty);
			array[3] = new TableDataGetter<RecipeDef>("workSpeedStat's skillNeedFactors", delegate(RecipeDef d)
			{
				string result;
				if (d.workSpeedStat == null)
				{
					result = string.Empty;
				}
				else if (d.workSpeedStat.skillNeedFactors.NullOrEmpty<SkillNeed>())
				{
					result = string.Empty;
				}
				else
				{
					result = (from fac in d.workSpeedStat.skillNeedFactors
					select fac.skill.defName).ToCommaList(false);
				}
				return result;
			});
			array[4] = new TableDataGetter<RecipeDef>("workSkillLearnFactor", (RecipeDef d) => d.workSkillLearnFactor);
			DebugTables.MakeTablesDialog<RecipeDef>(allDefs, array);
		}

		[Category("Economy")]
		[DebugOutput]
		public static void Drugs()
		{
			Func<ThingDef, float> realIngredientCost = (ThingDef d) => DebugOutputsEconomy.CostToMake(d, true);
			Func<ThingDef, float> realSellPrice = (ThingDef d) => d.BaseMarketValue * 0.6f;
			Func<ThingDef, float> realBuyPrice = (ThingDef d) => d.BaseMarketValue * 1.4f;
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

		[Category("Economy")]
		[DebugOutput]
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

		[Category("Economy")]
		[DebugOutput]
		public static void AnimalGrowth()
		{
			DebugOutputsEconomy.<AnimalGrowth>c__AnonStorey2 <AnimalGrowth>c__AnonStorey = new DebugOutputsEconomy.<AnimalGrowth>c__AnonStorey2();
			DebugOutputsEconomy.<AnimalGrowth>c__AnonStorey2 <AnimalGrowth>c__AnonStorey2 = <AnimalGrowth>c__AnonStorey;
			if (DebugOutputsEconomy.<>f__mg$cache0 == null)
			{
				DebugOutputsEconomy.<>f__mg$cache0 = new Func<ThingDef, float>(DebugOutputsEconomy.GestationDaysEach);
			}
			<AnimalGrowth>c__AnonStorey2.gestDaysEach = DebugOutputsEconomy.<>f__mg$cache0;
			<AnimalGrowth>c__AnonStorey.nutritionToGestate = delegate(ThingDef d)
			{
				float num = 0f;
				LifeStageAge lifeStageAge = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1];
				return num + <AnimalGrowth>c__AnonStorey.gestDaysEach(d) * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
			};
			<AnimalGrowth>c__AnonStorey.babyMeatNut = delegate(ThingDef d)
			{
				LifeStageAge lifeStageAge = d.race.lifeStageAges[0];
				return d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f * lifeStageAge.def.bodySizeFactor;
			};
			<AnimalGrowth>c__AnonStorey.babyMeatNutPerInput = ((ThingDef d) => <AnimalGrowth>c__AnonStorey.babyMeatNut(d) / <AnimalGrowth>c__AnonStorey.nutritionToGestate(d));
			<AnimalGrowth>c__AnonStorey.nutritionToAdulthood = delegate(ThingDef d)
			{
				float num = 0f;
				num += <AnimalGrowth>c__AnonStorey.nutritionToGestate(d);
				for (int i = 1; i < d.race.lifeStageAges.Count; i++)
				{
					LifeStageAge lifeStageAge = d.race.lifeStageAges[i];
					float num2 = lifeStageAge.minAge - d.race.lifeStageAges[i - 1].minAge;
					float num3 = num2 * 60f;
					num += num3 * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
				}
				return num;
			};
			<AnimalGrowth>c__AnonStorey.adultMeatNutPerInput = ((ThingDef d) => d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f / <AnimalGrowth>c__AnonStorey.nutritionToAdulthood(d));
			<AnimalGrowth>c__AnonStorey.bestMeatPerInput = delegate(ThingDef d)
			{
				float a = <AnimalGrowth>c__AnonStorey.babyMeatNutPerInput(d);
				float b = <AnimalGrowth>c__AnonStorey.adultMeatNutPerInput(d);
				return Mathf.Max(a, b);
			};
			<AnimalGrowth>c__AnonStorey.eggNut = delegate(ThingDef d)
			{
				CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				if (compProperties == null)
				{
					return string.Empty;
				}
				return compProperties.eggFertilizedDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2");
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh
			orderby <AnimalGrowth>c__AnonStorey.bestMeatPerInput(d) descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[17];
			array[0] = new TableDataGetter<ThingDef>(string.Empty, (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => d.race.baseHungerRate.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("gestDaysEach", (ThingDef d) => <AnimalGrowth>c__AnonStorey.gestDaysEach(d).ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("herbiv", (ThingDef d) => ((d.race.foodType & FoodTypeFlags.Plant) == FoodTypeFlags.None) ? string.Empty : "Y");
			array[4] = new TableDataGetter<ThingDef>("|", (ThingDef d) => "|");
			array[5] = new TableDataGetter<ThingDef>("bodySize", (ThingDef d) => d.race.baseBodySize.ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("age Adult", (ThingDef d) => d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge.ToString("F2"));
			array[7] = new TableDataGetter<ThingDef>("nutrition to adulthood", (ThingDef d) => <AnimalGrowth>c__AnonStorey.nutritionToAdulthood(d).ToString("F2"));
			array[8] = new TableDataGetter<ThingDef>("adult meat-nut", (ThingDef d) => (d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("adult meat-nut / input-nut", (ThingDef d) => <AnimalGrowth>c__AnonStorey.adultMeatNutPerInput(d).ToString("F3"));
			array[10] = new TableDataGetter<ThingDef>("|", (ThingDef d) => "|");
			array[11] = new TableDataGetter<ThingDef>("baby size", (ThingDef d) => (d.race.lifeStageAges[0].def.bodySizeFactor * d.race.baseBodySize).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("nutrition to gestate", (ThingDef d) => <AnimalGrowth>c__AnonStorey.nutritionToGestate(d).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("egg nut", (ThingDef d) => <AnimalGrowth>c__AnonStorey.eggNut(d));
			array[14] = new TableDataGetter<ThingDef>("baby meat-nut", (ThingDef d) => <AnimalGrowth>c__AnonStorey.babyMeatNut(d).ToString("F2"));
			array[15] = new TableDataGetter<ThingDef>("baby meat-nut / input-nut", (ThingDef d) => <AnimalGrowth>c__AnonStorey.babyMeatNutPerInput(d).ToString("F2"));
			array[16] = new TableDataGetter<ThingDef>("baby wins", (ThingDef d) => (<AnimalGrowth>c__AnonStorey.babyMeatNutPerInput(d) <= <AnimalGrowth>c__AnonStorey.adultMeatNutPerInput(d)) ? string.Empty : "B");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[Category("Economy")]
		[DebugOutput]
		public static void AnimalBreeding()
		{
			IEnumerable<ThingDef> source = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh
			select d;
			if (DebugOutputsEconomy.<>f__mg$cache1 == null)
			{
				DebugOutputsEconomy.<>f__mg$cache1 = new Func<ThingDef, float>(DebugOutputsEconomy.GestationDaysEach);
			}
			IEnumerable<ThingDef> dataSources = source.OrderByDescending(DebugOutputsEconomy.<>f__mg$cache1);
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>(string.Empty, (ThingDef d) => d.defName);
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

		private static float GestationDaysEach(ThingDef d)
		{
			if (d.HasComp(typeof(CompEggLayer)))
			{
				CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				return compProperties.eggLayIntervalDays / compProperties.eggCountRange.Average;
			}
			return d.race.gestationPeriodDays / ((d.race.litterSizeCurve == null) ? 1f : Rand.ByCurveAverage(d.race.litterSizeCurve));
		}

		[Category("Economy")]
		[DebugOutput]
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

		[Category("Economy")]
		[DebugOutput]
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
			array[10] = new TableDataGetter<ThingDef>("nutrition per growDay", (ThingDef d) => (d.plant.harvestedThingDef.ingestible == null) ? string.Empty : (d.plant.harvestYield * d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.plant.growDays).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => (d.plant.harvestedThingDef.ingestible == null) ? string.Empty : d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("fertMin", (ThingDef d) => d.plant.fertilityMin.ToStringPercent());
			array[13] = new TableDataGetter<ThingDef>("fertSensitivity", (ThingDef d) => d.plant.fertilitySensitivity.ToStringPercent());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[Category("Economy")]
		[DebugOutput]
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
				if (list.Count == 0)
				{
					return string.Empty;
				}
				return list.ToCommaList(false);
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
				if (list.Count == 0)
				{
					return string.Empty;
				}
				return list.ToCommaList(false);
			};
			Func<ThingDef, string> calculatedMarketValue = delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.Producible(d))
				{
					return "not producible";
				}
				if (!d.StatBaseDefined(StatDefOf.MarketValue))
				{
					return "used";
				}
				string text = StatWorker_MarketValue.CalculatedBaseMarketValue(d, null).ToString("F1");
				if (StatWorker_MarketValue.CalculableRecipe(d) != null)
				{
					return text + " (recipe)";
				}
				return text;
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
			array[11] = new TableDataGetter<ThingDef>("thing set\nmaker tags", (ThingDef d) => (!d.thingSetMakerTags.NullOrEmpty<string>()) ? d.thingSetMakerTags.ToCommaList(false) : string.Empty);
			array[12] = new TableDataGetter<ThingDef>("made\nfrom\nstuff", (ThingDef d) => d.MadeFromStuff.ToStringCheckBlank());
			array[13] = new TableDataGetter<ThingDef>("cost list", (ThingDef d) => DebugOutputsEconomy.CostListString(d, false, false));
			array[14] = new TableDataGetter<ThingDef>("recipes", (ThingDef d) => recipes(d));
			array[15] = new TableDataGetter<ThingDef>("work amount\nsources", (ThingDef d) => workAmountSources(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[Category("Economy")]
		[DebugOutput]
		public static void ItemAccessibility()
		{
			IEnumerable<ThingDef> dataSources = from x in ThingSetMakerUtility.allGeneratableItems
			orderby x.defName
			select x;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("1", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1, Find.CurrentMap)) ? string.Empty : "✓");
			array[2] = new TableDataGetter<ThingDef>("10", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10, Find.CurrentMap)) ? string.Empty : "✓");
			array[3] = new TableDataGetter<ThingDef>("100", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 100, Find.CurrentMap)) ? string.Empty : "✓");
			array[4] = new TableDataGetter<ThingDef>("1000", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1000, Find.CurrentMap)) ? string.Empty : "✓");
			array[5] = new TableDataGetter<ThingDef>("10000", (ThingDef d) => (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10000, Find.CurrentMap)) ? string.Empty : "✓");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[Category("Economy")]
		[DebugOutput]
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

		[Category("Economy")]
		[DebugOutput]
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
			array[9] = new TableDataGetter<RecipeDef>("min skill", (RecipeDef d) => (!d.skillRequirements.NullOrEmpty<SkillRequirement>()) ? d.skillRequirements[0].Summary : string.Empty);
			array[10] = new TableDataGetter<RecipeDef>("cheapest stuff", (RecipeDef d) => (DebugOutputsEconomy.CheapestNonDerpStuff(d) == null) ? string.Empty : DebugOutputsEconomy.CheapestNonDerpStuff(d).defName);
			array[11] = new TableDataGetter<RecipeDef>("cheapest ingredients", (RecipeDef d) => (from pa in DebugOutputsEconomy.CheapestIngredients(d)
			select pa.First.defName + " x" + pa.Second).ToCommaList(false));
			DebugTables.MakeTablesDialog<RecipeDef>(dataSources, array);
		}

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

		public static string CostListString(BuildableDef d, bool divideByVolume, bool starIfOnlyBuyable)
		{
			if (!DebugOutputsEconomy.Producible(d))
			{
				return string.Empty;
			}
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
			return list.ToCommaList(false);
		}

		private static float TrueWorkWithCarryTime(RecipeDef d)
		{
			ThingDef stuffDef = DebugOutputsEconomy.CheapestNonDerpStuff(d);
			return (float)d.ingredients.Count * 90f + d.WorkAmountTotal(stuffDef) + 90f;
		}

		private static float CheapestIngredientValue(RecipeDef d)
		{
			float num = 0f;
			foreach (Pair<ThingDef, float> pair in DebugOutputsEconomy.CheapestIngredients(d))
			{
				num += pair.First.BaseMarketValue * pair.Second;
			}
			return num;
		}

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

		private static float WorkValueEstimate(RecipeDef d)
		{
			return DebugOutputsEconomy.TrueWorkWithCarryTime(d) * 0.01f;
		}

		private static ThingDef CheapestNonDerpStuff(RecipeDef d)
		{
			ThingDef productDef = d.products[0].thingDef;
			if (!productDef.MadeFromStuff)
			{
				return null;
			}
			return (from td in d.ingredients.First<IngredientCount>().filter.AllowedThingDefs
			where !productDef.IsWeapon || !PawnWeaponGenerator.IsDerpWeapon(productDef, td)
			select td).MinBy((ThingDef td) => td.BaseMarketValue / td.VolumePerUnit);
		}

		private static float CheapestProductsValue(RecipeDef d)
		{
			float num = 0f;
			foreach (ThingDefCountClass thingDefCountClass in d.products)
			{
				num += thingDefCountClass.thingDef.GetStatValueAbstract(StatDefOf.MarketValue, DebugOutputsEconomy.CheapestNonDerpStuff(d)) * (float)thingDefCountClass.count;
			}
			return num;
		}

		private static string CostToMakeString(ThingDef d, bool real = false)
		{
			if (d.recipeMaker == null)
			{
				return "-";
			}
			return DebugOutputsEconomy.CostToMake(d, real).ToString("F1");
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
				foreach (ThingDefCountClass thingDefCountClass in d.costList)
				{
					float num2 = 1f;
					if (real)
					{
						num2 = ((!DebugOutputsEconomy.RequiresBuying(thingDefCountClass.thingDef)) ? 0.6f : 1.4f);
					}
					num += (float)thingDefCountClass.count * DebugOutputsEconomy.CostToMake(thingDefCountClass.thingDef, true) * num2;
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
				foreach (ThingDefCountClass thingDefCountClass in def.costList)
				{
					if (DebugOutputsEconomy.RequiresBuying(thingDefCountClass.thingDef))
					{
						return true;
					}
				}
				return false;
			}
			return !DefDatabase<ThingDef>.AllDefs.Any((ThingDef d) => d.plant != null && d.plant.harvestedThingDef == def && d.plant.Sowable);
		}

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
			if (num != 3.40282347E+38f)
			{
				return num;
			}
			return -1f;
		}

		[CompilerGenerated]
		private static string <RecipeSkills>m__0(RecipeDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <RecipeSkills>m__1(RecipeDef d)
		{
			return (d.workSkill != null) ? d.workSkill.defName : string.Empty;
		}

		[CompilerGenerated]
		private static string <RecipeSkills>m__2(RecipeDef d)
		{
			return (d.workSpeedStat != null) ? d.workSpeedStat.defName : string.Empty;
		}

		[CompilerGenerated]
		private static string <RecipeSkills>m__3(RecipeDef d)
		{
			string result;
			if (d.workSpeedStat == null)
			{
				result = string.Empty;
			}
			else if (d.workSpeedStat.skillNeedFactors.NullOrEmpty<SkillNeed>())
			{
				result = string.Empty;
			}
			else
			{
				result = (from fac in d.workSpeedStat.skillNeedFactors
				select fac.skill.defName).ToCommaList(false);
			}
			return result;
		}

		[CompilerGenerated]
		private static float <RecipeSkills>m__4(RecipeDef d)
		{
			return d.workSkillLearnFactor;
		}

		[CompilerGenerated]
		private static float <Drugs>m__5(ThingDef d)
		{
			return DebugOutputsEconomy.CostToMake(d, true);
		}

		[CompilerGenerated]
		private static float <Drugs>m__6(ThingDef d)
		{
			return d.BaseMarketValue * 0.6f;
		}

		[CompilerGenerated]
		private static float <Drugs>m__7(ThingDef d)
		{
			return d.BaseMarketValue * 1.4f;
		}

		[CompilerGenerated]
		private static bool <Drugs>m__8(ThingDef d)
		{
			return d.IsWithinCategory(ThingCategoryDefOf.Medicine) || d.IsWithinCategory(ThingCategoryDefOf.Drugs);
		}

		[CompilerGenerated]
		private static string <Drugs>m__9(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Drugs>m__A(ThingDef d)
		{
			return DebugOutputsEconomy.CostListString(d, true, true);
		}

		[CompilerGenerated]
		private static string <Drugs>m__B(ThingDef d)
		{
			return DebugOutputsEconomy.WorkToProduceBest(d).ToString("F0");
		}

		[CompilerGenerated]
		private static bool <Wool>m__C(ThingDef d)
		{
			return d.category == ThingCategory.Pawn && d.race.IsFlesh && d.GetCompProperties<CompProperties_Shearable>() != null;
		}

		[CompilerGenerated]
		private static string <Wool>m__D(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Wool>m__E(ThingDef d)
		{
			return d.GetCompProperties<CompProperties_Shearable>().woolDef.defName;
		}

		[CompilerGenerated]
		private static string <Wool>m__F(ThingDef d)
		{
			return d.GetCompProperties<CompProperties_Shearable>().woolAmount.ToString();
		}

		[CompilerGenerated]
		private static string <Wool>m__10(ThingDef d)
		{
			return d.GetCompProperties<CompProperties_Shearable>().woolDef.BaseMarketValue.ToString("F2");
		}

		[CompilerGenerated]
		private static string <Wool>m__11(ThingDef d)
		{
			return d.GetCompProperties<CompProperties_Shearable>().shearIntervalDays.ToString("F1");
		}

		[CompilerGenerated]
		private static string <Wool>m__12(ThingDef d)
		{
			CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
			return (compProperties.woolDef.BaseMarketValue * (float)compProperties.woolAmount * (60f / (float)compProperties.shearIntervalDays)).ToString("F0");
		}

		[CompilerGenerated]
		private static float <AnimalGrowth>m__13(ThingDef d)
		{
			LifeStageAge lifeStageAge = d.race.lifeStageAges[0];
			return d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f * lifeStageAge.def.bodySizeFactor;
		}

		[CompilerGenerated]
		private static string <AnimalGrowth>m__14(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return string.Empty;
			}
			return compProperties.eggFertilizedDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2");
		}

		[CompilerGenerated]
		private static bool <AnimalGrowth>m__15(ThingDef d)
		{
			return d.category == ThingCategory.Pawn && d.race.IsFlesh;
		}

		[CompilerGenerated]
		private static string <AnimalGrowth>m__16(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <AnimalGrowth>m__17(ThingDef d)
		{
			return d.race.baseHungerRate.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalGrowth>m__18(ThingDef d)
		{
			return ((d.race.foodType & FoodTypeFlags.Plant) == FoodTypeFlags.None) ? string.Empty : "Y";
		}

		[CompilerGenerated]
		private static string <AnimalGrowth>m__19(ThingDef d)
		{
			return "|";
		}

		[CompilerGenerated]
		private static string <AnimalGrowth>m__1A(ThingDef d)
		{
			return d.race.baseBodySize.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalGrowth>m__1B(ThingDef d)
		{
			return d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalGrowth>m__1C(ThingDef d)
		{
			return (d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f).ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalGrowth>m__1D(ThingDef d)
		{
			return "|";
		}

		[CompilerGenerated]
		private static string <AnimalGrowth>m__1E(ThingDef d)
		{
			return (d.race.lifeStageAges[0].def.bodySizeFactor * d.race.baseBodySize).ToString("F2");
		}

		[CompilerGenerated]
		private static bool <AnimalBreeding>m__1F(ThingDef d)
		{
			return d.category == ThingCategory.Pawn && d.race.IsFlesh;
		}

		[CompilerGenerated]
		private static string <AnimalBreeding>m__20(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <AnimalBreeding>m__21(ThingDef d)
		{
			return DebugOutputsEconomy.GestationDaysEach(d).ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalBreeding>m__22(ThingDef d)
		{
			return (!d.HasComp(typeof(CompEggLayer))) ? ((d.race.litterSizeCurve == null) ? 1f : Rand.ByCurveAverage(d.race.litterSizeCurve)).ToString("F2") : d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalBreeding>m__23(ThingDef d)
		{
			return (!d.HasComp(typeof(CompEggLayer))) ? d.race.gestationPeriodDays.ToString("F1") : d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays.ToString("F1");
		}

		[CompilerGenerated]
		private static string <AnimalBreeding>m__24(ThingDef d)
		{
			float f = 1f + ((!d.HasComp(typeof(CompEggLayer))) ? ((d.race.litterSizeCurve == null) ? 1f : Rand.ByCurveAverage(d.race.litterSizeCurve)) : d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average);
			float num = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge * 60f;
			float num2 = num + ((!d.HasComp(typeof(CompEggLayer))) ? d.race.gestationPeriodDays : d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays);
			float p = 30f / num2;
			return Mathf.Pow(f, p).ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalBreeding>m__25(ThingDef d)
		{
			float f = 1f + ((!d.HasComp(typeof(CompEggLayer))) ? ((d.race.litterSizeCurve == null) ? 1f : Rand.ByCurveAverage(d.race.litterSizeCurve)) : d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average);
			float num = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge * 60f;
			float num2 = num + ((!d.HasComp(typeof(CompEggLayer))) ? d.race.gestationPeriodDays : d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays);
			float p = 60f / num2;
			return Mathf.Pow(f, p).ToString("F2");
		}

		[CompilerGenerated]
		private static bool <BuildingSkills>m__26(BuildableDef d)
		{
			return d.BuildableByPlayer;
		}

		[CompilerGenerated]
		private static string <BuildingSkills>m__27(BuildableDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static int <BuildingSkills>m__28(BuildableDef d)
		{
			return d.constructionSkillPrerequisite;
		}

		[CompilerGenerated]
		private static float <Crops>m__29(ThingDef d)
		{
			float num = 1.1f;
			num += d.plant.growDays * 1f;
			return num + (d.plant.sowWork + d.plant.harvestWork) * 0.00612f;
		}

		[CompilerGenerated]
		private static bool <Crops>m__2A(ThingDef d)
		{
			return d.category == ThingCategory.Plant && d.plant.Harvestable && d.plant.Sowable;
		}

		[CompilerGenerated]
		private static bool <Crops>m__2B(ThingDef d)
		{
			return d.plant.IsTree;
		}

		[CompilerGenerated]
		private static string <Crops>m__2C(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Crops>m__2D(ThingDef d)
		{
			return d.plant.harvestedThingDef.defName;
		}

		[CompilerGenerated]
		private static string <Crops>m__2E(ThingDef d)
		{
			return d.plant.growDays.ToString("F1");
		}

		[CompilerGenerated]
		private static string <Crops>m__2F(ThingDef d)
		{
			return (d.plant.sowWork + d.plant.harvestWork).ToString("F0");
		}

		[CompilerGenerated]
		private static string <Crops>m__30(ThingDef d)
		{
			return d.plant.harvestYield.ToString("F1");
		}

		[CompilerGenerated]
		private static string <Crops>m__31(ThingDef d)
		{
			return d.plant.harvestedThingDef.BaseMarketValue.ToString("F2");
		}

		[CompilerGenerated]
		private static string <Crops>m__32(ThingDef d)
		{
			return (d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue).ToString("F2");
		}

		[CompilerGenerated]
		private static string <Crops>m__33(ThingDef d)
		{
			return (d.plant.harvestedThingDef.ingestible == null) ? string.Empty : (d.plant.harvestYield * d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.plant.growDays).ToString("F2");
		}

		[CompilerGenerated]
		private static string <Crops>m__34(ThingDef d)
		{
			return (d.plant.harvestedThingDef.ingestible == null) ? string.Empty : d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <Crops>m__35(ThingDef d)
		{
			return d.plant.fertilityMin.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <Crops>m__36(ThingDef d)
		{
			return d.plant.fertilitySensitivity.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__37(ThingDef d)
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
			if (list.Count == 0)
			{
				return string.Empty;
			}
			return list.ToCommaList(false);
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__38(ThingDef d)
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
			if (list.Count == 0)
			{
				return string.Empty;
			}
			return list.ToCommaList(false);
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__39(ThingDef d)
		{
			if (!DebugOutputsEconomy.Producible(d))
			{
				return "not producible";
			}
			if (!d.StatBaseDefined(StatDefOf.MarketValue))
			{
				return "used";
			}
			string text = StatWorker_MarketValue.CalculatedBaseMarketValue(d, null).ToString("F1");
			if (StatWorker_MarketValue.CalculableRecipe(d) != null)
			{
				return text + " (recipe)";
			}
			return text;
		}

		[CompilerGenerated]
		private static bool <ItemAndBuildingAcquisition>m__3A(ThingDef d)
		{
			return (d.category == ThingCategory.Item && d.BaseMarketValue > 0.01f) || (d.category == ThingCategory.Building && (d.BuildableByPlayer || d.Minifiable));
		}

		[CompilerGenerated]
		private static float <ItemAndBuildingAcquisition>m__3B(ThingDef d)
		{
			return d.BaseMarketValue;
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__3C(ThingDef d)
		{
			return d.category.ToString().Substring(0, 1).CapitalizeFirst();
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__3D(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__3E(ThingDef d)
		{
			return (d.category == ThingCategory.Item || d.Minifiable).ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__3F(ThingDef d)
		{
			return d.BaseMarketValue.ToString("F1");
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__40(ThingDef d)
		{
			return DebugOutputsEconomy.CostToMakeString(d, false);
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__41(ThingDef d)
		{
			return (DebugOutputsEconomy.WorkToProduceBest(d) <= 0f) ? "-" : DebugOutputsEconomy.WorkToProduceBest(d).ToString("F1");
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__42(ThingDef d)
		{
			return (d.BaseMarketValue - DebugOutputsEconomy.CostToMake(d, false)).ToString("F1");
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__43(ThingDef d)
		{
			return (d.recipeMaker == null) ? "-" : ((d.BaseMarketValue - DebugOutputsEconomy.CostToMake(d, false)) / DebugOutputsEconomy.WorkToProduceBest(d) * 10000f).ToString("F0");
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__44(ThingDef d)
		{
			return d.statBases.Any((StatModifier st) => st.stat == StatDefOf.MarketValue).ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__45(ThingDef d)
		{
			return DebugOutputsEconomy.Producible(d).ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__46(ThingDef d)
		{
			return (!d.thingSetMakerTags.NullOrEmpty<string>()) ? d.thingSetMakerTags.ToCommaList(false) : string.Empty;
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__47(ThingDef d)
		{
			return d.MadeFromStuff.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <ItemAndBuildingAcquisition>m__48(ThingDef d)
		{
			return DebugOutputsEconomy.CostListString(d, false, false);
		}

		[CompilerGenerated]
		private static string <ItemAccessibility>m__49(ThingDef x)
		{
			return x.defName;
		}

		[CompilerGenerated]
		private static string <ItemAccessibility>m__4A(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <ItemAccessibility>m__4B(ThingDef d)
		{
			return (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1, Find.CurrentMap)) ? string.Empty : "✓";
		}

		[CompilerGenerated]
		private static string <ItemAccessibility>m__4C(ThingDef d)
		{
			return (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10, Find.CurrentMap)) ? string.Empty : "✓";
		}

		[CompilerGenerated]
		private static string <ItemAccessibility>m__4D(ThingDef d)
		{
			return (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 100, Find.CurrentMap)) ? string.Empty : "✓";
		}

		[CompilerGenerated]
		private static string <ItemAccessibility>m__4E(ThingDef d)
		{
			return (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1000, Find.CurrentMap)) ? string.Empty : "✓";
		}

		[CompilerGenerated]
		private static string <ItemAccessibility>m__4F(ThingDef d)
		{
			return (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10000, Find.CurrentMap)) ? string.Empty : "✓";
		}

		[CompilerGenerated]
		private static bool <Recipes>m__50(RecipeDef d)
		{
			return !d.products.NullOrEmpty<ThingDefCountClass>() && !d.ingredients.NullOrEmpty<IngredientCount>();
		}

		[CompilerGenerated]
		private static string <Recipes>m__51(RecipeDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Recipes>m__52(RecipeDef d)
		{
			return DebugOutputsEconomy.TrueWorkWithCarryTime(d).ToString("F0");
		}

		[CompilerGenerated]
		private static string <Recipes>m__53(RecipeDef d)
		{
			return (DebugOutputsEconomy.TrueWorkWithCarryTime(d) / 60f).ToString("F0");
		}

		[CompilerGenerated]
		private static string <Recipes>m__54(RecipeDef d)
		{
			return DebugOutputsEconomy.CheapestProductsValue(d).ToString("F1");
		}

		[CompilerGenerated]
		private static string <Recipes>m__55(RecipeDef d)
		{
			return DebugOutputsEconomy.CheapestIngredientValue(d).ToString("F1");
		}

		[CompilerGenerated]
		private static string <Recipes>m__56(RecipeDef d)
		{
			return DebugOutputsEconomy.WorkValueEstimate(d).ToString("F1");
		}

		[CompilerGenerated]
		private static string <Recipes>m__57(RecipeDef d)
		{
			return (DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.CheapestIngredientValue(d)).ToString("F1");
		}

		[CompilerGenerated]
		private static string <Recipes>m__58(RecipeDef d)
		{
			return (DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.WorkValueEstimate(d) - DebugOutputsEconomy.CheapestIngredientValue(d)).ToString("F1");
		}

		[CompilerGenerated]
		private static string <Recipes>m__59(RecipeDef d)
		{
			return ((DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.CheapestIngredientValue(d)) * 60000f / DebugOutputsEconomy.TrueWorkWithCarryTime(d)).ToString("F0");
		}

		[CompilerGenerated]
		private static string <Recipes>m__5A(RecipeDef d)
		{
			return (!d.skillRequirements.NullOrEmpty<SkillRequirement>()) ? d.skillRequirements[0].Summary : string.Empty;
		}

		[CompilerGenerated]
		private static string <Recipes>m__5B(RecipeDef d)
		{
			return (DebugOutputsEconomy.CheapestNonDerpStuff(d) == null) ? string.Empty : DebugOutputsEconomy.CheapestNonDerpStuff(d).defName;
		}

		[CompilerGenerated]
		private static string <Recipes>m__5C(RecipeDef d)
		{
			return (from pa in DebugOutputsEconomy.CheapestIngredients(d)
			select pa.First.defName + " x" + pa.Second).ToCommaList(false);
		}

		[CompilerGenerated]
		private static float <CheapestNonDerpStuff>m__5D(ThingDef td)
		{
			return td.BaseMarketValue / td.VolumePerUnit;
		}

		[CompilerGenerated]
		private static string <RecipeSkills>m__5E(SkillNeed fac)
		{
			return fac.skill.defName;
		}

		[CompilerGenerated]
		private static bool <ItemAndBuildingAcquisition>m__5F(StatModifier st)
		{
			return st.stat == StatDefOf.MarketValue;
		}

		[CompilerGenerated]
		private static string <Recipes>m__60(Pair<ThingDef, float> pa)
		{
			return pa.First.defName + " x" + pa.Second;
		}

		[CompilerGenerated]
		private sealed class <Drugs>c__AnonStorey1
		{
			internal Func<ThingDef, float> realIngredientCost;

			internal Func<ThingDef, float> realSellPrice;

			internal Func<ThingDef, float> realBuyPrice;

			public <Drugs>c__AnonStorey1()
			{
			}

			internal string <>m__0(ThingDef d)
			{
				return this.realIngredientCost(d).ToString("F1");
			}

			internal string <>m__1(ThingDef d)
			{
				return this.realSellPrice(d).ToString("F1");
			}

			internal string <>m__2(ThingDef d)
			{
				return (this.realSellPrice(d) - this.realIngredientCost(d)).ToString("F1");
			}

			internal string <>m__3(ThingDef d)
			{
				return ((this.realSellPrice(d) - this.realIngredientCost(d)) / DebugOutputsEconomy.WorkToProduceBest(d) * 30000f).ToString("F1");
			}

			internal string <>m__4(ThingDef d)
			{
				return this.realBuyPrice(d).ToString("F1");
			}
		}

		[CompilerGenerated]
		private sealed class <AnimalGrowth>c__AnonStorey2
		{
			internal Func<ThingDef, float> gestDaysEach;

			internal Func<ThingDef, float> babyMeatNut;

			internal Func<ThingDef, float> nutritionToGestate;

			internal Func<ThingDef, float> nutritionToAdulthood;

			internal Func<ThingDef, float> babyMeatNutPerInput;

			internal Func<ThingDef, float> adultMeatNutPerInput;

			internal Func<ThingDef, float> bestMeatPerInput;

			internal Func<ThingDef, string> eggNut;

			public <AnimalGrowth>c__AnonStorey2()
			{
			}

			internal float <>m__0(ThingDef d)
			{
				float num = 0f;
				LifeStageAge lifeStageAge = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1];
				return num + this.gestDaysEach(d) * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
			}

			internal float <>m__1(ThingDef d)
			{
				return this.babyMeatNut(d) / this.nutritionToGestate(d);
			}

			internal float <>m__2(ThingDef d)
			{
				float num = 0f;
				num += this.nutritionToGestate(d);
				for (int i = 1; i < d.race.lifeStageAges.Count; i++)
				{
					LifeStageAge lifeStageAge = d.race.lifeStageAges[i];
					float num2 = lifeStageAge.minAge - d.race.lifeStageAges[i - 1].minAge;
					float num3 = num2 * 60f;
					num += num3 * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
				}
				return num;
			}

			internal float <>m__3(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f / this.nutritionToAdulthood(d);
			}

			internal float <>m__4(ThingDef d)
			{
				float a = this.babyMeatNutPerInput(d);
				float b = this.adultMeatNutPerInput(d);
				return Mathf.Max(a, b);
			}

			internal float <>m__5(ThingDef d)
			{
				return this.bestMeatPerInput(d);
			}

			internal string <>m__6(ThingDef d)
			{
				return this.gestDaysEach(d).ToString("F2");
			}

			internal string <>m__7(ThingDef d)
			{
				return this.nutritionToAdulthood(d).ToString("F2");
			}

			internal string <>m__8(ThingDef d)
			{
				return this.adultMeatNutPerInput(d).ToString("F3");
			}

			internal string <>m__9(ThingDef d)
			{
				return this.nutritionToGestate(d).ToString("F2");
			}

			internal string <>m__A(ThingDef d)
			{
				return this.eggNut(d);
			}

			internal string <>m__B(ThingDef d)
			{
				return this.babyMeatNut(d).ToString("F2");
			}

			internal string <>m__C(ThingDef d)
			{
				return this.babyMeatNutPerInput(d).ToString("F2");
			}

			internal string <>m__D(ThingDef d)
			{
				return (this.babyMeatNutPerInput(d) <= this.adultMeatNutPerInput(d)) ? string.Empty : "B";
			}
		}

		[CompilerGenerated]
		private sealed class <Crops>c__AnonStorey3
		{
			internal Func<ThingDef, float> workCost;

			public <Crops>c__AnonStorey3()
			{
			}

			internal string <>m__0(ThingDef d)
			{
				return this.workCost(d).ToString("F2");
			}

			internal string <>m__1(ThingDef d)
			{
				return (this.workCost(d) / d.plant.harvestYield).ToString("F2");
			}

			internal string <>m__2(ThingDef d)
			{
				return ((d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue - this.workCost(d)) / d.plant.growDays).ToString("F2");
			}
		}

		[CompilerGenerated]
		private sealed class <ItemAndBuildingAcquisition>c__AnonStorey4
		{
			internal Func<ThingDef, string> calculatedMarketValue;

			internal Func<ThingDef, string> recipes;

			internal Func<ThingDef, string> workAmountSources;

			public <ItemAndBuildingAcquisition>c__AnonStorey4()
			{
			}

			internal string <>m__0(ThingDef d)
			{
				return this.calculatedMarketValue(d);
			}

			internal string <>m__1(ThingDef d)
			{
				return this.recipes(d);
			}

			internal string <>m__2(ThingDef d)
			{
				return this.workAmountSources(d);
			}
		}

		[CompilerGenerated]
		private sealed class <Producible>c__AnonStorey5
		{
			internal ThingDef d;

			public <Producible>c__AnonStorey5()
			{
			}

			internal bool <>m__0(RecipeDef r)
			{
				return r.products.Any((ThingDefCountClass pr) => pr.thingDef == this.d);
			}

			internal bool <>m__1(ThingDefCountClass pr)
			{
				return pr.thingDef == this.d;
			}
		}

		[CompilerGenerated]
		private sealed class <CheapestIngredients>c__Iterator0 : IEnumerable, IEnumerable<Pair<ThingDef, float>>, IEnumerator, IDisposable, IEnumerator<Pair<ThingDef, float>>
		{
			internal RecipeDef d;

			internal List<IngredientCount>.Enumerator $locvar0;

			internal IngredientCount <ing>__1;

			internal ThingDef <thing>__2;

			internal Pair<ThingDef, float> $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<ThingDef, bool> <>f__am$cache0;

			private static Func<ThingDef, float> <>f__am$cache1;

			[DebuggerHidden]
			public <CheapestIngredients>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = d.ingredients.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						ing = enumerator.Current;
						thing = (from td in ing.filter.AllowedThingDefs
						where td != ThingDefOf.Meat_Human
						select td).MinBy((ThingDef td) => td.BaseMarketValue / td.VolumePerUnit);
						this.$current = new Pair<ThingDef, float>(thing, ing.GetBaseCount() / d.IngredientValueGetter.ValuePerUnitOf(thing));
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			Pair<ThingDef, float> IEnumerator<Pair<ThingDef, float>>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Pair<Verse.ThingDef,float>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pair<ThingDef, float>> IEnumerable<Pair<ThingDef, float>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DebugOutputsEconomy.<CheapestIngredients>c__Iterator0 <CheapestIngredients>c__Iterator = new DebugOutputsEconomy.<CheapestIngredients>c__Iterator0();
				<CheapestIngredients>c__Iterator.d = d;
				return <CheapestIngredients>c__Iterator;
			}

			private static bool <>m__0(ThingDef td)
			{
				return td != ThingDefOf.Meat_Human;
			}

			private static float <>m__1(ThingDef td)
			{
				return td.BaseMarketValue / td.VolumePerUnit;
			}
		}

		[CompilerGenerated]
		private sealed class <CheapestNonDerpStuff>c__AnonStorey6
		{
			internal ThingDef productDef;

			public <CheapestNonDerpStuff>c__AnonStorey6()
			{
			}

			internal bool <>m__0(ThingDef td)
			{
				return !this.productDef.IsWeapon || !PawnWeaponGenerator.IsDerpWeapon(this.productDef, td);
			}
		}

		[CompilerGenerated]
		private sealed class <RequiresBuying>c__AnonStorey7
		{
			internal ThingDef def;

			public <RequiresBuying>c__AnonStorey7()
			{
			}

			internal bool <>m__0(ThingDef d)
			{
				return d.plant != null && d.plant.harvestedThingDef == this.def && d.plant.Sowable;
			}
		}
	}
}
