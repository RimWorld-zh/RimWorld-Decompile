using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_Standard : ItemCollectionGenerator
	{
		private static List<ThingStuffPair> newCandidates = new List<ThingStuffPair>();

		private static List<ThingDef> allowedFiltered = new List<ThingDef>();

		private static Dictionary<ThingDef, List<ThingDef>> allowedStuff = new Dictionary<ThingDef, List<ThingDef>>();

		protected override ItemCollectionGeneratorParams RandomTestParams
		{
			get
			{
				ItemCollectionGeneratorParams randomTestParams = base.RandomTestParams;
				randomTestParams.count = Rand.RangeInclusive(5, 10);
				randomTestParams.techLevel = TechLevel.Transcendent;
				if (base.def == ItemCollectionGeneratorDefOf.AIPersonaCores || base.def == ItemCollectionGeneratorDefOf.Neurotrainers)
				{
					randomTestParams.totalMarketValue = 0f;
				}
				else
				{
					randomTestParams.totalMarketValue = Rand.Range(3000f, 8000f);
				}
				return randomTestParams;
			}
		}

		protected virtual IEnumerable<ThingDef> AllowedDefs(ItemCollectionGeneratorParams parms)
		{
			return base.def.allowedDefs;
		}

		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			TechLevel techLevel = parms.techLevel;
			int count = parms.count;
			float totalMarketValue = parms.totalMarketValue;
			IEnumerable<ThingDef> enumerable = this.AllowedDefs(parms);
			if (enumerable.Any())
			{
				if (totalMarketValue > 0.0)
				{
					List<ThingStuffPair> list = this.GenerateDefsWithPossibleTotalMarketValue(count, totalMarketValue, enumerable, techLevel, 100);
					for (int i = 0; i < list.Count; i++)
					{
						ThingStuffPair thingStuffPair = list[i];
						ThingDef thing = thingStuffPair.thing;
						ThingStuffPair thingStuffPair2 = list[i];
						outThings.Add(ThingMaker.MakeThing(thing, thingStuffPair2.stuff));
					}
					this.IncreaseStackCountsToMarketValue(outThings, totalMarketValue);
				}
				else
				{
					for (int num = 0; num < count; num++)
					{
						ThingDef thingDef = enumerable.RandomElement();
						outThings.Add(ThingMaker.MakeThing(thingDef, GenStuff.RandomStuffFor(thingDef)));
					}
				}
				ItemCollectionGeneratorUtility.AssignRandomBaseGenItemQuality(outThings);
			}
		}

		private List<ThingStuffPair> GenerateDefsWithPossibleTotalMarketValue(int count, float totalMarketValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, int tries = 100)
		{
			List<ThingStuffPair> list = new List<ThingStuffPair>();
			ItemCollectionGenerator_Standard.allowedFiltered.Clear();
			ItemCollectionGenerator_Standard.allowedFiltered.AddRange(allowed);
			ItemCollectionGenerator_Standard.allowedFiltered.RemoveAll((Predicate<ThingDef>)delegate(ThingDef x)
			{
				if ((int)x.techLevel > (int)techLevel)
				{
					return true;
				}
				if (!x.MadeFromStuff && this.GetMinValue(new ThingStuffPair(x, null, 1f)) > totalMarketValue)
				{
					return true;
				}
				return false;
			});
			ItemCollectionGenerator_Standard.allowedStuff.Clear();
			for (int i = 0; i < ItemCollectionGenerator_Standard.allowedFiltered.Count; i++)
			{
				ThingDef thingDef = ItemCollectionGenerator_Standard.allowedFiltered[i];
				if (thingDef.MadeFromStuff)
				{
					List<ThingDef> list2 = default(List<ThingDef>);
					if (!ItemCollectionGenerator_Standard.allowedStuff.TryGetValue(thingDef, out list2))
					{
						list2 = new List<ThingDef>();
						ItemCollectionGenerator_Standard.allowedStuff.Add(thingDef, list2);
					}
					foreach (ThingDef item in GenStuff.AllowedStuffsFor(thingDef))
					{
						if (!(item.stuffProps.commonality <= 0.0) && (int)item.techLevel <= (int)techLevel && !(this.GetMinValue(new ThingStuffPair(thingDef, item, 1f)) > totalMarketValue))
						{
							list2.Add(item);
						}
					}
				}
			}
			ItemCollectionGenerator_Standard.allowedFiltered.RemoveAll((Predicate<ThingDef>)((ThingDef x) => x.MadeFromStuff && !ItemCollectionGenerator_Standard.allowedStuff[x].Any()));
			if (!ItemCollectionGenerator_Standard.allowedFiltered.Any())
			{
				return list;
			}
			float num = 0f;
			int num2 = 0;
			while (num2 < tries)
			{
				float num3 = 0f;
				float num4 = 0f;
				ItemCollectionGenerator_Standard.newCandidates.Clear();
				for (int num5 = 0; num5 < count; num5++)
				{
					ThingDef thingDef2 = ItemCollectionGenerator_Standard.allowedFiltered.RandomElement();
					ThingDef stuff = (!thingDef2.MadeFromStuff) ? null : ItemCollectionGenerator_Standard.allowedStuff[thingDef2].RandomElementByWeight((Func<ThingDef, float>)((ThingDef x) => x.stuffProps.commonality));
					ThingStuffPair thingStuffPair = new ThingStuffPair(thingDef2, stuff, 1f);
					ItemCollectionGenerator_Standard.newCandidates.Add(thingStuffPair);
					num3 += this.GetMinValue(thingStuffPair);
					num4 += this.GetMaxValue(thingStuffPair);
				}
				float num6 = (float)((!(num3 <= totalMarketValue)) ? (num3 - totalMarketValue) : 0.0);
				float num7 = (float)((!(num4 >= totalMarketValue)) ? (totalMarketValue - num4) : 0.0);
				if (!list.Any() || num > num6 + num7)
				{
					list.Clear();
					list.AddRange(ItemCollectionGenerator_Standard.newCandidates);
					num = num6 + num7;
				}
				if (!(num <= 0.0))
				{
					num2++;
					continue;
				}
				break;
			}
			return list;
		}

		private void IncreaseStackCountsToMarketValue(List<Thing> things, float totalMarketValue)
		{
			float num = 0f;
			for (int i = 0; i < things.Count; i++)
			{
				num += things[i].MarketValue * (float)things[i].stackCount;
			}
			if (!(num >= totalMarketValue))
			{
				float num2 = (totalMarketValue - num) / (float)things.Count;
				for (int j = 0; j < things.Count; j++)
				{
					float marketValue = things[j].MarketValue;
					int a = Mathf.FloorToInt(num2 / marketValue);
					int num3 = Mathf.Min(a, things[j].def.stackLimit - things[j].stackCount);
					if (num3 > 0)
					{
						things[j].stackCount += num3;
						num += marketValue * (float)num3;
					}
				}
				if (!(num >= totalMarketValue))
				{
					for (int k = 0; k < things.Count; k++)
					{
						float marketValue2 = things[k].MarketValue;
						int a2 = Mathf.FloorToInt((totalMarketValue - num) / marketValue2);
						int num4 = Mathf.Min(a2, things[k].def.stackLimit - things[k].stackCount);
						if (num4 > 0)
						{
							things[k].stackCount += num4;
							num += marketValue2 * (float)num4;
						}
					}
				}
			}
		}

		private float GetMinValue(ThingStuffPair thingDef)
		{
			return thingDef.thing.GetStatValueAbstract(StatDefOf.MarketValue, thingDef.stuff);
		}

		private float GetMaxValue(ThingStuffPair thingDef)
		{
			return this.GetMinValue(thingDef) * (float)thingDef.thing.stackLimit;
		}
	}
}
