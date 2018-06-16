using System;

namespace Verse.Sound
{
	// Token: 0x02000B8C RID: 2956
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06004034 RID: 16436 RVA: 0x0021C578 File Offset: 0x0021A978
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x0021C594 File Offset: 0x0021A994
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
