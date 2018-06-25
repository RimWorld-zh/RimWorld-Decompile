using System;

namespace Verse.Sound
{
	// Token: 0x02000B8A RID: 2954
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600403B RID: 16443 RVA: 0x0021CDC4 File Offset: 0x0021B1C4
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0021CDE0 File Offset: 0x0021B1E0
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
