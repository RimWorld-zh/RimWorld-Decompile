using System;

namespace Verse.Sound
{
	// Token: 0x02000B8D RID: 2957
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06004039 RID: 16441 RVA: 0x0021C6B0 File Offset: 0x0021AAB0
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x0021C6CC File Offset: 0x0021AACC
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}
