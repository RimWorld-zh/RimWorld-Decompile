using System;

namespace Verse.Sound
{
	// Token: 0x02000B89 RID: 2953
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600402D RID: 16429 RVA: 0x0021C524 File Offset: 0x0021A924
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x0021C540 File Offset: 0x0021A940
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
