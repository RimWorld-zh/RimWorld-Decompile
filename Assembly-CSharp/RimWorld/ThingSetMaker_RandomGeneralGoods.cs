using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006FA RID: 1786
	public class ThingSetMaker_RandomGeneralGoods : ThingSetMaker
	{
		// Token: 0x060026E1 RID: 9953 RVA: 0x0014D498 File Offset: 0x0014B898
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			IntRange? countRange = parms.countRange;
			IntRange intRange = (countRange == null) ? new IntRange(10, 20) : countRange.Value;
			TechLevel? techLevel = parms.techLevel;
			TechLevel techLevel2 = (techLevel == null) ? TechLevel.Undefined : techLevel.Value;
			int num = Mathf.Max(intRange.RandomInRange, 1);
			for (int i = 0; i < num; i++)
			{
				outThings.Add(this.GenerateSingle(techLevel2));
			}
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x0014D528 File Offset: 0x0014B928
		private Thing GenerateSingle(TechLevel techLevel)
		{
			Thing thing = null;
			int num = 0;
			while (thing == null && num < 50)
			{
				IEnumerable<Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>> goodsWeights = ThingSetMaker_RandomGeneralGoods.GoodsWeights;
				switch (goodsWeights.RandomElementByWeight((Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float> x) => x.Second).First)
				{
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Meals:
					thing = this.RandomMeals(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood:
					thing = this.RandomRawFood(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine:
					thing = this.RandomMedicine(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs:
					thing = this.RandomDrugs(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Resources:
					thing = this.RandomResources(techLevel);
					break;
				default:
					throw new Exception();
				}
				num++;
			}
			return thing;
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x0014D5F8 File Offset: 0x0014B9F8
		private Thing RandomMeals(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (techLevel.IsNeolithicOrWorse())
			{
				thingDef = ThingDefOf.Pemmican;
			}
			else
			{
				float value = Rand.Value;
				if (value < 0.5f)
				{
					thingDef = ThingDefOf.MealSimple;
				}
				else if ((double)value < 0.75)
				{
					thingDef = ThingDefOf.MealFine;
				}
				else
				{
					thingDef = ThingDefOf.MealSurvivalPack;
				}
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 10);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x0014D68C File Offset: 0x0014BA8C
		private Thing RandomRawFood(TechLevel techLevel)
		{
			ThingDef thingDef;
			Thing result;
			if (!this.PossibleRawFood(techLevel).TryRandomElement(out thingDef))
			{
				result = null;
			}
			else
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				int max = Mathf.Min(thingDef.stackLimit, 75);
				thing.stackCount = Rand.RangeInclusive(1, max);
				result = thing;
			}
			return result;
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x0014D6E0 File Offset: 0x0014BAE0
		private IEnumerable<ThingDef> PossibleRawFood(TechLevel techLevel)
		{
			return from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsNutritionGivingIngestible && !x.IsCorpse && x.ingestible.HumanEdible && !x.HasComp(typeof(CompHatcher)) && x.techLevel <= techLevel && x.ingestible.preferability < FoodPreferability.MealAwful
			select x;
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x0014D718 File Offset: 0x0014BB18
		private Thing RandomMedicine(TechLevel techLevel)
		{
			bool flag = Rand.Value < 0.75f && techLevel >= ThingDefOf.MedicineHerbal.techLevel;
			ThingDef thingDef;
			if (flag)
			{
				thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
				where x.IsMedicine && x.techLevel <= techLevel
				select x).MaxBy((ThingDef x) => x.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			}
			else if (!(from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsMedicine
			select x).TryRandomElement(out thingDef))
			{
				return null;
			}
			if (techLevel.IsNeolithicOrWorse())
			{
				thingDef = ThingDefOf.MedicineHerbal;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int max = Mathf.Min(thingDef.stackLimit, 20);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x0014D824 File Offset: 0x0014BC24
		private Thing RandomDrugs(TechLevel techLevel)
		{
			ThingDef thingDef;
			Thing result;
			if (!(from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsDrug && x.techLevel <= techLevel
			select x).TryRandomElement(out thingDef))
			{
				result = null;
			}
			else
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				int max = Mathf.Min(thingDef.stackLimit, 25);
				thing.stackCount = Rand.RangeInclusive(1, max);
				result = thing;
			}
			return result;
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x0014D898 File Offset: 0x0014BC98
		private Thing RandomResources(TechLevel techLevel)
		{
			ThingDef thingDef = BaseGenUtility.RandomCheapWallStuff(techLevel, false);
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x0014D8DC File Offset: 0x0014BCDC
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0.<AllGeneratableThingsDebugSub>c__AnonStorey4 <AllGeneratableThingsDebugSub>c__AnonStorey = new ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0.<AllGeneratableThingsDebugSub>c__AnonStorey4();
			<AllGeneratableThingsDebugSub>c__AnonStorey.<>f__ref$0 = this;
			ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0.<AllGeneratableThingsDebugSub>c__AnonStorey4 <AllGeneratableThingsDebugSub>c__AnonStorey2 = <AllGeneratableThingsDebugSub>c__AnonStorey;
			TechLevel? techLevel = parms.techLevel;
			<AllGeneratableThingsDebugSub>c__AnonStorey2.techLevel = ((techLevel == null) ? TechLevel.Undefined : techLevel.Value);
			if (<AllGeneratableThingsDebugSub>c__AnonStorey.techLevel.IsNeolithicOrWorse())
			{
				yield return ThingDefOf.Pemmican;
			}
			else
			{
				yield return ThingDefOf.MealSimple;
				yield return ThingDefOf.MealFine;
				yield return ThingDefOf.MealSurvivalPack;
			}
			foreach (ThingDef t in this.PossibleRawFood(<AllGeneratableThingsDebugSub>c__AnonStorey.techLevel))
			{
				yield return t;
			}
			foreach (ThingDef t2 in from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsMedicine
			select x)
			{
				yield return t2;
			}
			foreach (ThingDef t3 in from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsDrug && x.techLevel <= <AllGeneratableThingsDebugSub>c__AnonStorey.techLevel
			select x)
			{
				yield return t3;
			}
			if (<AllGeneratableThingsDebugSub>c__AnonStorey.techLevel.IsNeolithicOrWorse())
			{
				yield return ThingDefOf.WoodLog;
			}
			else
			{
				foreach (ThingDef t4 in from d in DefDatabase<ThingDef>.AllDefsListForReading
				where BaseGenUtility.IsCheapWallStuff(d)
				select d)
				{
					yield return t4;
				}
			}
			yield break;
		}

		// Token: 0x0400159C RID: 5532
		private static Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[] GoodsWeights = new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[]
		{
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Meals, 1f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood, 0.75f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Resources, 0.234f)
		};

		// Token: 0x020006FB RID: 1787
		private enum GoodsType
		{
			// Token: 0x040015A1 RID: 5537
			None,
			// Token: 0x040015A2 RID: 5538
			Meals,
			// Token: 0x040015A3 RID: 5539
			RawFood,
			// Token: 0x040015A4 RID: 5540
			Medicine,
			// Token: 0x040015A5 RID: 5541
			Drugs,
			// Token: 0x040015A6 RID: 5542
			Resources
		}
	}
}
