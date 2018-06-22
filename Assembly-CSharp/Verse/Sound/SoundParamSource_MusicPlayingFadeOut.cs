using System;

namespace Verse.Sound
{
	// Token: 0x02000B88 RID: 2952
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06004038 RID: 16440 RVA: 0x0021CCE8 File Offset: 0x0021B0E8
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x0021CD04 File Offset: 0x0021B104
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
