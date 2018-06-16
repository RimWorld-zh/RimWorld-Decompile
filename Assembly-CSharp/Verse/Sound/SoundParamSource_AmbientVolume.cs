using System;

namespace Verse.Sound
{
	// Token: 0x02000B8D RID: 2957
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06004037 RID: 16439 RVA: 0x0021C5DC File Offset: 0x0021A9DC
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x0021C5F8 File Offset: 0x0021A9F8
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}
