using System;
using Verse;

namespace RimWorld
{
	public class ThingSetMaker_Conditional_MinMaxTotalMarketValue : ThingSetMaker_Conditional
	{
		public float minMaxTotalMarketValue;

		public ThingSetMaker_Conditional_MinMaxTotalMarketValue()
		{
		}

		protected override bool Condition(ThingSetMakerParams parms)
		{
			FloatRange? totalMarketValueRange = parms.totalMarketValueRange;
			return totalMarketValueRange != null && parms.totalMarketValueRange.Value.max >= this.minMaxTotalMarketValue;
		}
	}
}
