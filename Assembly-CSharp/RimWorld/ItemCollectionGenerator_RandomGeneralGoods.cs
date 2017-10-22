using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_RandomGeneralGoods : ItemCollectionGenerator
	{
		private enum GoodsType
		{
			None = 0,
			Meals = 1,
			RawFood = 2,
			Medicine = 3,
			Drugs = 4,
			Resources = 5
		}

		private static Pair<GoodsType, float>[] GoodsWeights = new Pair<GoodsType, float>[5]
		{
			new Pair<GoodsType, float>(GoodsType.Meals, 1f),
			new Pair<GoodsType, float>(GoodsType.RawFood, 0.75f),
			new Pair<GoodsType, float>(GoodsType.Medicine, 0.234f),
			new Pair<GoodsType, float>(GoodsType.Drugs, 0.234f),
			new Pair<GoodsType, float>(GoodsType.Resources, 0.234f)
		};

		protected override ItemCollectionGeneratorParams RandomTestParams
		{
			get
			{
				ItemCollectionGeneratorParams randomTestParams = base.RandomTestParams;
				randomTestParams.count = Rand.RangeInclusive(10, 20);
				randomTestParams.techLevel = TechLevel.Transcendent;
				return randomTestParams;
			}
		}

		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			int count = parms.count;
			TechLevel techLevel = parms.techLevel;
			for (int num = 0; num < count; num++)
			{
				outThings.Add(this.GenerateSingle(techLevel));
			}
		}

		private Thing GenerateSingle(TechLevel techLevel)
		{
			GoodsType first = ItemCollectionGenerator_RandomGeneralGoods.GoodsWeights.RandomElementByWeight((Func<Pair<GoodsType, float>, float>)((Pair<GoodsType, float> x) => x.Second)).First;
			switch (first)
			{
			case GoodsType.Meals:
			{
				return this.RandomMeals(techLevel);
			}
			case GoodsType.RawFood:
			{
				return this.RandomRawFood(techLevel);
			}
			case GoodsType.Medicine:
			{
				return this.RandomMedicine(techLevel);
			}
			case GoodsType.Drugs:
			{
				return this.RandomDrugs(techLevel);
			}
			case GoodsType.Resources:
			{
				return this.RandomResources(techLevel);
			}
			default:
			{
				Log.Error("Goods type not handled: " + first);
				return null;
			}
			}
		}

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
				thingDef = ((!(value < 0.5)) ? ((!((double)value < 0.75)) ? ThingDefOf.MealSurvivalPack : ThingDefOf.MealFine) : ThingDefOf.MealSimple);
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 10);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		private Thing RandomRawFood(TechLevel techLevel)
		{
			ThingDef thingDef = default(ThingDef);
			if (!(from x in ItemCollectionGeneratorUtility.allGeneratableItems
			where x.IsNutritionGivingIngestible && !x.IsCorpse && ThingDefOf.Human.race.CanEverEat(x) && !x.HasComp(typeof(CompHatcher)) && (int)x.techLevel <= (int)techLevel && (int)x.ingestible.preferability < 5
			select x).TryRandomElement<ThingDef>(out thingDef))
			{
				return null;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int max = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		private Thing RandomMedicine(TechLevel techLevel)
		{
			ThingDef herbalMedicine = default(ThingDef);
			if (techLevel.IsNeolithicOrWorse())
			{
				herbalMedicine = ThingDefOf.HerbalMedicine;
			}
			else if (!(from x in ItemCollectionGeneratorUtility.allGeneratableItems
			where x.IsMedicine && (int)x.techLevel <= (int)techLevel
			select x).TryRandomElement<ThingDef>(out herbalMedicine))
			{
				return null;
			}
			Thing thing = ThingMaker.MakeThing(herbalMedicine, null);
			int max = Mathf.Min(herbalMedicine.stackLimit, 20);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		private Thing RandomDrugs(TechLevel techLevel)
		{
			ThingDef thingDef = default(ThingDef);
			if (!(from x in ItemCollectionGeneratorUtility.allGeneratableItems
			where x.IsDrug && (int)x.techLevel <= (int)techLevel
			select x).TryRandomElement<ThingDef>(out thingDef))
			{
				return null;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int max = Mathf.Min(thingDef.stackLimit, 25);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		private Thing RandomResources(TechLevel techLevel)
		{
			ThingDef thingDef = BaseGenUtility.RandomCheapWallStuff(techLevel, false);
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}
	}
}
