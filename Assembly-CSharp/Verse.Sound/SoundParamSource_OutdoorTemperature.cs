namespace Verse.Sound
{
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		public override float ValueFor(Sample samp)
		{
			return (float)((Current.ProgramState == ProgramState.Playing) ? ((Find.VisibleMap != null) ? Find.VisibleMap.mapTemperature.OutdoorTemp : 0.0) : 0.0);
		}
	}
}
