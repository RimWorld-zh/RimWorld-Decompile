namespace Verse.Sound
{
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		public override float ValueFor(Sample samp)
		{
			return (float)((Current.ProgramState == ProgramState.Playing && Find.MusicManagerPlay != null) ? Find.MusicManagerPlay.subtleAmbienceSoundVolumeMultiplier : 1.0);
		}
	}
}
