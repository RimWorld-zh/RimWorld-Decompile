using System;

namespace Verse.Sound
{
	// Token: 0x02000B8B RID: 2955
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600403B RID: 16443 RVA: 0x0021D0A4 File Offset: 0x0021B4A4
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0021D0C0 File Offset: 0x0021B4C0
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
