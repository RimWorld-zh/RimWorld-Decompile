using System;

namespace Verse.Sound
{
	// Token: 0x02000B88 RID: 2952
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x0021CF7C File Offset: 0x0021B37C
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x0021CF98 File Offset: 0x0021B398
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
