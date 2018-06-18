using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F4 RID: 1780
	public class ThingSetMaker_MarketValue : ThingSetMaker
	{
		// Token: 0x060026B0 RID: 9904 RVA: 0x0014B520 File Offset: 0x00149920
		public ThingSetMaker_MarketValue()
		{
			this.nextSeed = Rand.Int;
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x0014B534 File Offset: 0x00149934
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			bool result;
			if (!this.AllowedThingDefs(parms).Any<ThingDef>())
			{
				result = false;
			}
			else
			{
				IntRange? countRange = parms.countRange;
				if (countRange != null && parms.countRange.Value.max <= 0)
				{
					result = false;
				}
				else
				{
					FloatRange? totalMarketValueRange = parms.totalMarketValueRange;
					if (totalMarketValueRange == null || parms.totalMarketValueRange.Value.max <= 0f)
					{
						result = false;
					}
					else
					{
						float? maxTotalMass = parms.maxTotalMass;
						if (maxTotalMass != null && parms.maxTotalMass != 3.40282347E+38f)
						{
							IEnumerable<ThingDef> candidates = this.AllowedThingDefs(parms);
							TechLevel? techLevel = parms.techLevel;
							TechLevel stuffTechLevel = (techLevel == null) ? TechLevel.Undefined : techLevel.Value;
							float value = parms.maxTotalMass.Value;
							IntRange? countRange2 = parms.countRange;
							if (!ThingSetMakerUtility.PossibleToWeighNoMoreThan(candidates, stuffTechLevel, value, (countRange2 == null) ? 1 : parms.countRange.Value.min))
							{
								return false;
							}
						}
						float num;
						result = this.GeneratePossibleDefs(parms, out num, this.nextSeed).Any<ThingStuffPairWithQuality>();
					}
				}
			}
			return result;
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x0014B6AC File Offset: 0x00149AAC
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			float? maxTotalMass = parms.maxTotalMass;
			float maxMass = (maxTotalMass == null) ? float.MaxValue : maxTotalMass.Value;
			float totalValue;
			List<ThingStuffPairWithQuality> list = this.GeneratePossibleDefs(parms, out totalValue, this.nextSeed);
			for (int i = 0; i < list.Count; i++)
			{
				outThings.Add(list[i].MakeThing());
			}
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalValue, (Thing x) => x.MarketValue, maxMass);
			this.nextSeed++;
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x0014B758 File Offset: 0x00149B58
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0014B774 File Offset: 0x00149B74
		private float GetMinValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue);
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x0014B798 File Offset: 0x00149B98
		private float GetMaxValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return this.GetMinValue(thingStuffPair) * (float)thingStuffPair.thing.stackLimit;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x0014B7C4 File Offset: 0x00149BC4
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalMarketValue);
			Rand.PopState();
			return result;
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0014B7F0 File Offset: 0x00149BF0
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			List<ThingStuffPairWithQuality> result;
			if (!enumerable.Any<ThingDef>())
			{
				totalMarketValue = 0f;
				result = new List<ThingStuffPairWithQuality>();
			}
			else
			{
				TechLevel? techLevel = parms.techLevel;
				TechLevel techLevel2 = (techLevel == null) ? TechLevel.Undefined : techLevel.Value;
				IntRange? countRange = parms.countRange;
				IntRange intRange = (countRange == null) ? new IntRange(1, int.MaxValue) : countRange.Value;
				FloatRange? totalMarketValueRange = parms.totalMarketValueRange;
				FloatRange floatRange = (totalMarketValueRange == null) ? FloatRange.Zero : totalMarketValueRange.Value;
				float? maxTotalMass = parms.maxTotalMass;
				float num = (maxTotalMass == null) ? float.MaxValue : maxTotalMass.Value;
				QualityGenerator? qualityGenerator = parms.qualityGenerator;
				QualityGenerator qualityGenerator2 = (qualityGenerator == null) ? QualityGenerator.BaseGen : qualityGenerator.Value;
				totalMarketValue = floatRange.RandomInRange;
				IntRange countRange2 = intRange;
				float totalValue = totalMarketValue;
				IEnumerable<ThingDef> allowed = enumerable;
				TechLevel techLevel3 = techLevel2;
				QualityGenerator qualityGenerator3 = qualityGenerator2;
				Func<ThingStuffPairWithQuality, float> getMinValue = new Func<ThingStuffPairWithQuality, float>(this.GetMinValue);
				Func<ThingStuffPairWithQuality, float> getMaxValue = new Func<ThingStuffPairWithQuality, float>(this.GetMaxValue);
				float maxMass = num;
				result = ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange2, totalValue, allowed, techLevel3, qualityGenerator3, getMinValue, getMaxValue, null, 100, maxMass);
			}
			return result;
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x0014B944 File Offset: 0x00149D44
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			TechLevel? techLevel2 = parms.techLevel;
			TechLevel techLevel = (techLevel2 == null) ? TechLevel.Undefined : techLevel2.Value;
			foreach (ThingDef t in this.AllowedThingDefs(parms))
			{
				float? maxTotalMass = parms.maxTotalMass;
				if (maxTotalMass != null && parms.maxTotalMass != 3.40282347E+38f)
				{
					float? maxTotalMass2 = parms.maxTotalMass;
					if (ThingSetMakerUtility.GetMinMass(t, techLevel) > maxTotalMass2)
					{
						continue;
					}
				}
				FloatRange? totalMarketValueRange = parms.totalMarketValueRange;
				if (totalMarketValueRange == null || parms.totalMarketValueRange.Value.max == 3.40282347E+38f || ThingSetMakerUtility.GetMinMarketValue(t, techLevel) <= parms.totalMarketValueRange.Value.max)
				{
					yield return t;
				}
			}
			yield break;
		}

		// Token: 0x04001586 RID: 5510
		private int nextSeed;
	}
}
