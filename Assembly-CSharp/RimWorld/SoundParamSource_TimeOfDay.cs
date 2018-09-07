using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		public SoundParamSource_TimeOfDay()
		{
		}

		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		public override float ValueFor(Sample samp)
		{
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			return GenLocalDate.HourFloat(Find.CurrentMap);
		}
	}
}
