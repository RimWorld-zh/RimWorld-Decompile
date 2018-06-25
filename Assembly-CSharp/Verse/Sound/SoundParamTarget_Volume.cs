using System;

namespace Verse.Sound
{
	// Token: 0x02000B92 RID: 2962
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x0600404F RID: 16463 RVA: 0x0021D0DC File Offset: 0x0021B4DC
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0021D0F6 File Offset: 0x0021B4F6
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
