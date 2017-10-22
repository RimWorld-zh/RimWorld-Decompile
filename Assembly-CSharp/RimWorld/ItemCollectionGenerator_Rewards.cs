using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_Rewards : ItemCollectionGenerator
	{
		private class Option
		{
			public ThingDef thingDef;

			public QualityCategory quality;

			public ThingDef stuff;
		}

		private const float SilverChance = 0.5f;

		private const float SpecialRewardChance = 0.35f;

		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			int? count = parms.count;
			int num = (!count.HasValue) ? Rand.RangeInclusive(1, 3) : count.Value;
			float? totalMarketValue = parms.totalMarketValue;
			float num2 = (!totalMarketValue.HasValue) ? Rand.Range(1500f, 4000f) : totalMarketValue.Value;
			TechLevel? techLevel = parms.techLevel;
			TechLevel techLevel2 = (!techLevel.HasValue) ? TechLevel.Spacer : techLevel.Value;
			Predicate<ThingDef> validator = parms.validator;
			for (int num3 = 0; num3 < num; num3++)
			{
				outThings.Add(this.GenerateReward(num2 / (float)num, techLevel2, validator));
			}
		}

		private Thing GenerateReward(float value, TechLevel techLevel, Predicate<ThingDef> validator = null)
		{
			Thing result;
			ThingDef thingDef = default(ThingDef);
			if (Rand.Chance(0.5f))
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
				thing.stackCount = ThingUtility.RoundedResourceStackCount(Mathf.Max(GenMath.RoundRandom(value), 1));
				result = thing;
			}
			else if (Rand.Chance(0.35f) && (from x in ItemCollectionGeneratorUtility.allGeneratableItems
			where x.itemGeneratorTags != null && x.itemGeneratorTags.Contains(ItemCollectionGeneratorUtility.SpecialRewardTag) && Mathf.Abs((float)(1.0 - x.BaseMarketValue / value)) <= 0.34999999403953552
			select x).TryRandomElement<ThingDef>(out thingDef))
			{
				Thing thing2 = ThingMaker.MakeThing(thingDef, GenStuff.RandomStuffFor(thingDef));
				CompQuality compQuality = thing2.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					compQuality.SetQuality(QualityUtility.RandomBaseGenItemQuality(), ArtGenerationContext.Outsider);
				}
				result = thing2;
			}
			else
			{
				Option option2 = (from option in ItemCollectionGeneratorUtility.allGeneratableItems.Select((Func<ThingDef, Option>)delegate(ThingDef td)
				{
					Option result2;
					if ((int)td.techLevel > (int)techLevel)
					{
						result2 = null;
					}
					else if (td.itemGeneratorTags != null && td.itemGeneratorTags.Contains(ItemCollectionGeneratorUtility.SpecialRewardTag))
					{
						result2 = null;
					}
					else if (!td.IsWithinCategory(ThingCategoryDefOf.Apparel) && !td.IsWithinCategory(ThingCategoryDefOf.Weapons) && !td.IsWithinCategory(ThingCategoryDefOf.Art) && (td.building == null || !td.Minifiable) && (td.tradeTags == null || !td.tradeTags.Contains("Exotic")))
					{
						result2 = null;
					}
					else if ((object)validator != null && !validator(td))
					{
						result2 = null;
					}
					else
					{
						ThingDef stuff = null;
						if (td.MadeFromStuff && !GenStuff.TryRandomStuffByCommonalityFor(td, out stuff, techLevel))
						{
							result2 = null;
						}
						else
						{
							Option option3 = new Option();
							option3.thingDef = td;
							option3.quality = ((!td.HasComp(typeof(CompQuality))) ? QualityCategory.Normal : QualityUtility.RandomQuality());
							option3.stuff = stuff;
							result2 = option3;
						}
					}
					return result2;
				})
				where option != null
				select option).MinBy((Func<Option, float>)delegate(Option option)
				{
					float value2 = StatDefOf.MarketValue.Worker.GetValue(StatRequest.For(option.thingDef, option.stuff, option.quality), true);
					return Mathf.Abs(value - value2);
				});
				Thing thing3 = ThingMaker.MakeThing(option2.thingDef, option2.stuff);
				CompQuality compQuality2 = thing3.TryGetComp<CompQuality>();
				if (compQuality2 != null)
				{
					compQuality2.SetQuality(option2.quality, ArtGenerationContext.Outsider);
				}
				result = thing3;
			}
			return result;
		}
	}
}
