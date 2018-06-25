using System;

namespace Verse.Sound
{
	// Token: 0x02000B93 RID: 2963
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x0600404F RID: 16463 RVA: 0x0021D3BC File Offset: 0x0021B7BC
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0021D3D6 File Offset: 0x0021B7D6
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
