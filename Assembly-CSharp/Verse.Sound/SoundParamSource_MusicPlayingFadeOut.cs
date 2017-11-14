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
			if (Current.ProgramState == ProgramState.Playing && Find.MusicManagerPlay != null)
			{
				return Find.MusicManagerPlay.subtleAmbienceSoundVolumeMultiplier;
			}
			return 1f;
		}
	}
}
