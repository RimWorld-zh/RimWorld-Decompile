using System;

namespace Verse.Sound
{
	// Token: 0x02000B86 RID: 2950
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x0021CC24 File Offset: 0x0021B024
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x0021CC40 File Offset: 0x0021B040
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}
