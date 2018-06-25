using System;

namespace Verse
{
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		public float severityPerDayGrowing = 0f;

		public float severityPerDayRemission = 0f;

		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);

		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}
	}
}
