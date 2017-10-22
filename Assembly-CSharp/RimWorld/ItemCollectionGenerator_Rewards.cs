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

		protected override ItemCollectionGeneratorParams RandomTestParams
		{
			get
			{
				ItemCollectionGeneratorParams randomTestParams = base.RandomTestParams;
				randomTestParams.count = Rand.RangeInclusive(5, 8);
				randomTestParams.totalMarketValue = Rand.Range(10000f, 20000f);
				randomTestParams.techLevel = TechLevel.Transcendent;
				return randomTestParams;
			}
		}

		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			int count = parms.count;
			float totalMarketValue = parms.totalMarketValue;
			TechLevel techLevel = parms.techLevel;
			Predicate<ThingDef> validator = parms.validator;
			for (int num = 0; num < count; num++)
			{
				outThings.Add(this.GenerateReward(totalMarketValue / (float)count, techLevel, validator));
			}
		}

		private Thing GenerateReward(float value, TechLevel techLevel, Predicate<ThingDef> validator = null)
		{
			if (Rand.Value < 0.5)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
				thing.stackCount = Mathf.Max(GenMath.RoundRandom(value), 1);
				return thing;
			}
			Option option2 = (from option in ItemCollectionGeneratorUtility.allGeneratableItems.Select((Func<ThingDef, Option>)delegate(ThingDef td)
			{
				if ((int)td.techLevel > (int)techLevel)
				{
					return null;
				}
				if (!td.IsWithinCategory(ThingCategoryDefOf.Apparel) && !td.IsWithinCategory(ThingCategoryDefOf.Weapons) && !td.IsWithinCategory(ThingCategoryDefOf.Art) && (td.building == null || !td.Minifiable) && (td.tradeTags == null || !td.tradeTags.Contains("Exotic")))
				{
					return null;
				}
				if ((object)validator != null && !validator(td))
				{
					return null;
				}
				ThingDef stuff = null;
				if (td.MadeFromStuff && !(from x in GenStuff.AllowedStuffsFor(td)
				where (int)x.techLevel <= (int)techLevel
				select x).TryRandomElementByWeight<ThingDef>((Func<ThingDef, float>)((ThingDef st) => st.stuffProps.commonality), out stuff))
				{
					return null;
				}
				Option option3 = new Option();
				option3.thingDef = td;
				option3.quality = ((!td.HasComp(typeof(CompQuality))) ? QualityCategory.Normal : QualityUtility.RandomQuality());
				option3.stuff = stuff;
				return option3;
			})
			where option != null
			select option).MinBy((Func<Option, float>)delegate(Option option)
			{
				float value2 = StatDefOf.MarketValue.Worker.GetValue(StatRequest.For(option.thingDef, option.stuff, option.quality), true);
				return Mathf.Abs(value - value2);
			});
			Thing thing2 = ThingMaker.MakeThing(option2.thingDef, option2.stuff);
			if (option2.thingDef.HasComp(typeof(CompQuality)))
			{
				thing2.TryGetComp<CompQuality>().SetQuality(option2.quality, ArtGenerationContext.Outsider);
			}
			return thing2;
		}
	}
}
