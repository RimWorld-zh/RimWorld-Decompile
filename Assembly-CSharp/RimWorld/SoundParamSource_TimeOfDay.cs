using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		public override float ValueFor(Sample samp)
		{
			return (float)((Find.VisibleMap != null) ? GenLocalDate.HourFloat(Find.VisibleMap) : 0.0);
		}
	}
}
