using System;

namespace Verse.Sound
{
	// Token: 0x02000B94 RID: 2964
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06004048 RID: 16456 RVA: 0x0021C890 File Offset: 0x0021AC90
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0021C8AA File Offset: 0x0021ACAA
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
