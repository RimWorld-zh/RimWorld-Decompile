using System;

namespace Verse.Sound
{
	// Token: 0x02000B8A RID: 2954
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06004030 RID: 16432 RVA: 0x0021C588 File Offset: 0x0021A988
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x0021C5A4 File Offset: 0x0021A9A4
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}
