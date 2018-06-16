using System;

namespace Verse.Sound
{
	// Token: 0x02000B89 RID: 2953
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600402B RID: 16427 RVA: 0x0021C450 File Offset: 0x0021A850
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x0021C46C File Offset: 0x0021A86C
		public override float ValueFor(Sample samp)
		{
			float result;
			if (samp.ExternalParams.sizeAggregator == null)
			{
				result = 0f;
			}
			else
			{
				result = samp.ExternalParams.sizeAggregator.AggregateSize;
			}
			return result;
		}
	}
}
