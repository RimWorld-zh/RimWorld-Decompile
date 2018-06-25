using System;

namespace Verse.Sound
{
	// Token: 0x02000B89 RID: 2953
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06004035 RID: 16437 RVA: 0x0021CFE0 File Offset: 0x0021B3E0
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x0021CFFC File Offset: 0x0021B3FC
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}
