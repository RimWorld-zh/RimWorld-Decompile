using System;

namespace Verse.Sound
{
	// Token: 0x02000B8C RID: 2956
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x0600403E RID: 16446 RVA: 0x0021D108 File Offset: 0x0021B508
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x0021D124 File Offset: 0x0021B524
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}
