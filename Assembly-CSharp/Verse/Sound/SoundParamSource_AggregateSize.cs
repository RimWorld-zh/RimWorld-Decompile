using System;

namespace Verse.Sound
{
	// Token: 0x02000B85 RID: 2949
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600402F RID: 16431 RVA: 0x0021CBC0 File Offset: 0x0021AFC0
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x0021CBDC File Offset: 0x0021AFDC
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
