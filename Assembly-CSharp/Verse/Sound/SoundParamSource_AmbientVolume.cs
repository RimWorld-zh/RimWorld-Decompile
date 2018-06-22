using System;

namespace Verse.Sound
{
	// Token: 0x02000B89 RID: 2953
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x0600403B RID: 16443 RVA: 0x0021CD4C File Offset: 0x0021B14C
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0021CD68 File Offset: 0x0021B168
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}
