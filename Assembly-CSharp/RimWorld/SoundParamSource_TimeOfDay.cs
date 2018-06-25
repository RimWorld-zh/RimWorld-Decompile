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
			float result;
			if (Find.CurrentMap == null)
			{
				result = 0f;
			}
			else
			{
				result = GenLocalDate.HourFloat(Find.CurrentMap);
			}
			return result;
		}
	}
}
