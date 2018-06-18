using System;

namespace Verse.Sound
{
	// Token: 0x02000B8C RID: 2956
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06004036 RID: 16438 RVA: 0x0021C64C File Offset: 0x0021AA4C
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x0021C668 File Offset: 0x0021AA68
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
