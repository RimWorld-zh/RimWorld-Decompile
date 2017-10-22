using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ItemCollectionGeneratorByTotalValueUtility
	{
		private static List<ThingStuffPair> newCandidates = new List<ThingStuffPair>();

		private static List<ThingDef> allowedFiltered = new List<ThingDef>();

		private static Dictionary<ThingDef, List<ThingDef>> allowedStuff = new Dictionary<ThingDef, List<ThingDef>>();

		public static List<ThingStuffPair> GenerateDefsWithPossibleTotalValue(int count, float totalValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, Func<Thing, float> getValue, Func<ThingStuffPair, float> getMinValue, Func<ThingStuffPair, float> getMaxValue, Func<ThingDef, float> weightSelector = null, int tries = 100)
		{
			List<ThingStuffPair> list = new List<ThingStuffPair>();
			ItemCollectionGeneratorByTotalValueUtility.allowedFiltered.Clear();
			ItemCollectionGeneratorByTotalValueUtility.allowedFiltered.AddRange(allowed);
			ItemCollectionGeneratorByTotalValueUtility.allowedFiltered.RemoveAll((Predicate<ThingDef>)((ThingDef x) => (byte)(((int)x.techLevel > (int)techLevel) ? 1 : ((!x.MadeFromStuff && getMinValue(new ThingStuffPair(x, null, 1f)) > totalValue) ? 1 : 0)) != 0));
			ItemCollectionGeneratorByTotalValueUtility.allowedStuff.Clear();
			for (int i = 0; i < ItemCollectionGeneratorByTotalValueUtility.allowedFiltered.Count; i++)
			{
				ThingDef thingDef = ItemCollectionGeneratorByTotalValueUtility.allowedFiltered[i];
				if (thingDef.MadeFromStuff)
				{
					List<ThingDef> list2 = default(List<ThingDef>);
					if (!ItemCollectionGeneratorByTotalValueUtility.allowedStuff.TryGetValue(thingDef, out list2))
					{
						list2 = new List<ThingDef>();
						ItemCollectionGeneratorByTotalValueUtility.allowedStuff.Add(thingDef, list2);
					}
					foreach (ThingDef item in GenStuff.AllowedStuffsFor(thingDef))
					{
						if (!(item.stuffProps.commonality <= 0.0) && (int)item.techLevel <= (int)techLevel && !(getMinValue(new ThingStuffPair(thingDef, item, 1f)) > totalValue))
						{
							list2.Add(item);
						}
					}
				}
			}
			ItemCollectionGeneratorByTotalValueUtility.allowedFiltered.RemoveAll((Predicate<ThingDef>)((ThingDef x) => x.MadeFromStuff && !ItemCollectionGeneratorByTotalValueUtility.allowedStuff[x].Any()));
			List<ThingStuffPair> result;
			if (!ItemCollectionGeneratorByTotalValueUtility.allowedFiltered.Any())
			{
				result = list;
			}
			else
			{
				float num = 0f;
				int num2 = 0;
				while (num2 < tries)
				{
					float num3 = 0f;
					float num4 = 0f;
					ItemCollectionGeneratorByTotalValueUtility.newCandidates.Clear();
					for (int num5 = 0; num5 < count; num5++)
					{
						ThingDef thingDef2 = ((object)weightSelector == null) ? ItemCollectionGeneratorByTotalValueUtility.allowedFiltered.RandomElement() : ItemCollectionGeneratorByTotalValueUtility.allowedFiltered.RandomElementByWeight(weightSelector);
						ThingDef stuff = (!thingDef2.MadeFromStuff) ? null : ItemCollectionGeneratorByTotalValueUtility.allowedStuff[thingDef2].RandomElementByWeight((Func<ThingDef, float>)((ThingDef x) => x.stuffProps.commonality));
						ThingStuffPair thingStuffPair = new ThingStuffPair(thingDef2, stuff, 1f);
						ItemCollectionGeneratorByTotalValueUtility.newCandidates.Add(thingStuffPair);
						num3 += getMinValue(thingStuffPair);
						num4 += getMaxValue(thingStuffPair);
					}
					float num6 = (float)((!(num3 <= totalValue)) ? (num3 - totalValue) : 0.0);
					float num7 = (float)((!(num4 >= totalValue)) ? (totalValue - num4) : 0.0);
					if (!list.Any() || num > num6 + num7)
					{
						list.Clear();
						list.AddRange(ItemCollectionGeneratorByTotalValueUtility.newCandidates);
						num = num6 + num7;
					}
					if (!(num <= 0.0))
					{
						num2++;
						continue;
					}
					break;
				}
				result = list;
			}
			return result;
		}

		public static void IncreaseStackCountsToTotalValue(List<Thing> things, float totalValue, Func<Thing, float> getValue)
		{
			float num = 0f;
			for (int i = 0; i < things.Count; i++)
			{
				num += getValue(things[i]) * (float)things[i].stackCount;
			}
			if (!(num >= totalValue))
			{
				float num2 = (totalValue - num) / (float)things.Count;
				for (int j = 0; j < things.Count; j++)
				{
					float num3 = getValue(things[j]);
					int a = Mathf.FloorToInt(num2 / num3);
					int num4 = Mathf.Min(a, things[j].def.stackLimit - things[j].stackCount);
					if (num4 > 0)
					{
						things[j].stackCount += num4;
						num += num3 * (float)num4;
					}
				}
				if (!(num >= totalValue))
				{
					for (int k = 0; k < things.Count; k++)
					{
						float num5 = getValue(things[k]);
						int a2 = Mathf.FloorToInt((totalValue - num) / num5);
						int num6 = Mathf.Min(a2, things[k].def.stackLimit - things[k].stackCount);
						if (num6 > 0)
						{
							things[k].stackCount += num6;
							num += num5 * (float)num6;
						}
					}
				}
			}
		}
	}
}
