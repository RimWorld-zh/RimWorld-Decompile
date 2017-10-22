using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_Food : ItemCollectionGenerator
	{
		private static List<ThingDef> food = new List<ThingDef>();

		public static void Reset()
		{
			ItemCollectionGenerator_Food.food.Clear();
			ItemCollectionGenerator_Food.food.AddRange(from x in ItemCollectionGeneratorUtility.allGeneratableItems
			where x.IsNutritionGivingIngestible && !x.HasComp(typeof(CompHatcher)) && !x.IsDrug
			select x);
		}

		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			int? count = parms.count;
			int count2 = (!count.HasValue) ? Rand.RangeInclusive(3, 6) : count.Value;
			float? totalNutrition = parms.totalNutrition;
			float totalValue = (!totalNutrition.HasValue) ? Rand.Range(5f, 10f) : totalNutrition.Value;
			TechLevel? techLevel = parms.techLevel;
			TechLevel techLevel2 = (!techLevel.HasValue) ? TechLevel.Spacer : techLevel.Value;
			bool? nonHumanEdibleFoodAllowed = parms.nonHumanEdibleFoodAllowed;
			bool flag = nonHumanEdibleFoodAllowed.HasValue && parms.nonHumanEdibleFoodAllowed.Value;
			IEnumerable<ThingDef> enumerable = (!flag) ? (from x in ItemCollectionGenerator_Food.food
			where x.ingestible.HumanEdible
			select x) : ItemCollectionGenerator_Food.food;
			if (!flag)
			{
				enumerable = from x in enumerable
				where (int)x.ingestible.preferability > 1
				select x;
			}
			FoodPreferability? minPreferability = parms.minPreferability;
			if (minPreferability.HasValue)
			{
				enumerable = from x in enumerable
				where (int)x.ingestible.preferability >= (int)parms.minPreferability.Value
				select x;
			}
			if (enumerable.Any())
			{
				int numMeats = (from x in enumerable
				where x.IsMeat
				select x).Count();
				int numLeathers = (from x in enumerable
				where x.IsLeather
				select x).Count();
				Func<ThingDef, float> weightSelector = (Func<ThingDef, float>)((ThingDef x) => ItemCollectionGeneratorUtility.AdjustedSelectionWeight(x, numMeats, numLeathers));
				List<ThingStuffPair> list = ItemCollectionGeneratorByTotalValueUtility.GenerateDefsWithPossibleTotalValue(count2, totalValue, enumerable, techLevel2, (Func<Thing, float>)((Thing x) => x.def.ingestible.nutrition), (Func<ThingStuffPair, float>)((ThingStuffPair x) => x.thing.ingestible.nutrition), (Func<ThingStuffPair, float>)((ThingStuffPair x) => x.thing.ingestible.nutrition * (float)x.thing.stackLimit), weightSelector, 100);
				for (int i = 0; i < list.Count; i++)
				{
					ThingStuffPair thingStuffPair = list[i];
					ThingDef thing = thingStuffPair.thing;
					ThingStuffPair thingStuffPair2 = list[i];
					outThings.Add(ThingMaker.MakeThing(thing, thingStuffPair2.stuff));
				}
				ItemCollectionGeneratorByTotalValueUtility.IncreaseStackCountsToTotalValue(outThings, totalValue, (Func<Thing, float>)((Thing x) => x.def.ingestible.nutrition));
			}
		}
	}
}
