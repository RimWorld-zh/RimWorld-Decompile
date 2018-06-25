using System;

namespace Verse.Sound
{
	// Token: 0x02000B8B RID: 2955
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x0600403E RID: 16446 RVA: 0x0021CE28 File Offset: 0x0021B228
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x0021CE44 File Offset: 0x0021B244
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}
