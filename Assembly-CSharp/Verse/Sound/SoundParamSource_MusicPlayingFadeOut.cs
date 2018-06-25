using System;

namespace Verse.Sound
{
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		public SoundParamSource_MusicPlayingFadeOut()
		{
		}

		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		public override float ValueFor(Sample samp)
		{
			float result;
			if (Current.ProgramState != ProgramState.Playing || Find.MusicManagerPlay == null)
			{
				result = 1f;
			}
			else
			{
				result = Find.MusicManagerPlay.subtleAmbienceSoundVolumeMultiplier;
			}
			return result;
		}
	}
}
