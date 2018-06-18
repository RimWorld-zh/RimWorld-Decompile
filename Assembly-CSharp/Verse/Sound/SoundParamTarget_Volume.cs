using System;

namespace Verse.Sound
{
	// Token: 0x02000B94 RID: 2964
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x0600404A RID: 16458 RVA: 0x0021C964 File Offset: 0x0021AD64
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0021C97E File Offset: 0x0021AD7E
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
