using System;

namespace Verse.Sound
{
	// Token: 0x02000B90 RID: 2960
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x0600404C RID: 16460 RVA: 0x0021D000 File Offset: 0x0021B400
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x0021D01A File Offset: 0x0021B41A
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
