using System;

namespace Verse.Sound
{
	// Token: 0x02000B88 RID: 2952
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06004035 RID: 16437 RVA: 0x0021CD00 File Offset: 0x0021B100
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x0021CD1C File Offset: 0x0021B11C
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}
