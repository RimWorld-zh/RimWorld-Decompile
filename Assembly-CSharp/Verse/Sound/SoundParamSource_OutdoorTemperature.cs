using System;

namespace Verse.Sound
{
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		public SoundParamSource_OutdoorTemperature()
		{
		}

		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		public override float ValueFor(Sample samp)
		{
			float result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = 0f;
			}
			else if (Find.CurrentMap == null)
			{
				result = 0f;
			}
			else
			{
				result = Find.CurrentMap.mapTemperature.OutdoorTemp;
			}
			return result;
		}
	}
}
