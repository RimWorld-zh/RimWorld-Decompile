using System;

namespace Verse.Sound
{
	// Token: 0x02000B8A RID: 2954
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x0600402E RID: 16430 RVA: 0x0021C4B4 File Offset: 0x0021A8B4
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x0021C4D0 File Offset: 0x0021A8D0
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}
