using System;

namespace Verse
{
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		public float immunityPerDayNotSick = 0f;

		public float immunityPerDaySick = 0f;

		public float severityPerDayNotImmune = 0f;

		public float severityPerDayImmune = 0f;

		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);

		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}
	}
}
