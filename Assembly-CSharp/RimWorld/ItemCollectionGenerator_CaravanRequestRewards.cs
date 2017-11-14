using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_CaravanRequestRewards : ItemCollectionGenerator_Rewards
	{
		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			int? count = parms.count;
			parms.count = ((!count.HasValue) ? 1 : count.Value);
			float? totalMarketValue = parms.totalMarketValue;
			double value;
			if (totalMarketValue.HasValue)
			{
				value = totalMarketValue.Value;
			}
			else
			{
				IntRange baseValueWantedRange = IncidentWorker_CaravanRequest.BaseValueWantedRange;
				float num = (float)baseValueWantedRange.min;
				FloatRange rewardMarketValueFactorRange = IncidentWorker_CaravanRequest.RewardMarketValueFactorRange;
				float min = num * rewardMarketValueFactorRange.min;
				IntRange baseValueWantedRange2 = IncidentWorker_CaravanRequest.BaseValueWantedRange;
				float num2 = (float)baseValueWantedRange2.max;
				FloatRange rewardMarketValueFactorRange2 = IncidentWorker_CaravanRequest.RewardMarketValueFactorRange;
				FloatRange floatRange = new FloatRange(min, num2 * rewardMarketValueFactorRange2.max);
				value = floatRange.RandomInRange;
			}
			parms.totalMarketValue = (float)value;
			base.Generate(parms, outThings);
		}
	}
}
