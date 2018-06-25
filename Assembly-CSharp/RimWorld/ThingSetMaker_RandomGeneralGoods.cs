using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F8 RID: 1784
	public class ThingSetMaker_RandomGeneralGoods : ThingSetMaker
	{
		// Token: 0x0400159E RID: 5534
		private static Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[] GoodsWeights = new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[]
		{
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Meals, 1f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood, 0.75f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Resources, 0.234f)
		};

		// Token: 0x060026DC RID: 9948 RVA: 0x0014D9EC File Offset: 0x0014BDEC
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

		// Token: 0x060026DD RID: 9949 RVA: 0x0014DA7C File Offset: 0x0014BE7C
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

		// Token: 0x060026DE RID: 9950 RVA: 0x0014DB4C File Offset: 0x0014BF4C
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

		// Token: 0x060026DF RID: 9951 RVA: 0x0014DBE0 File Offset: 0x0014BFE0
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

		// Token: 0x060026E0 RID: 9952 RVA: 0x0014DC34 File Offset: 0x0014C034
		private IEnumerable<ThingDef> PossibleRawFood(TechLevel techLevel)
		{
			return from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsNutritionGivingIngestible && !x.IsCorpse && x.ingestible.HumanEdible && !x.HasComp(typeof(CompHatcher)) && x.techLevel <= techLevel && x.ingestible.preferability < FoodPreferability.MealAwful
			select x;
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x0014DC6C File Offset: 0x0014C06C
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

		// Token: 0x060026E2 RID: 9954 RVA: 0x0014DD78 File Offset: 0x0014C178
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

		// Token: 0x060026E3 RID: 9955 RVA: 0x0014DDEC File Offset: 0x0014C1EC
		private Thing RandomResources(TechLevel techLevel)
		{
			ThingDef thingDef = BaseGenUtility.RandomCheapWallStuff(techLevel, false);
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x0014DE30 File Offset: 0x0014C230
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

		// Token: 0x020006F9 RID: 1785
		private enum GoodsType
		{
			// Token: 0x040015A3 RID: 5539
			None,
			// Token: 0x040015A4 RID: 5540
			Meals,
			// Token: 0x040015A5 RID: 5541
			RawFood,
			// Token: 0x040015A6 RID: 5542
			Medicine,
			// Token: 0x040015A7 RID: 5543
			Drugs,
			// Token: 0x040015A8 RID: 5544
			Resources
		}
	}
}
