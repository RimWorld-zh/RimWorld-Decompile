using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B98 RID: 2968
	public class SoundParamTarget_PropertyHighPass : SoundParamTarget
	{
		// Token: 0x04002B2C RID: 11052
		private HighPassFilterProperty filterProperty;

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06004059 RID: 16473 RVA: 0x0021D234 File Offset: 0x0021B634
		public override string Label
		{
			get
			{
				return "HighPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x0600405A RID: 16474 RVA: 0x0021D260 File Offset: 0x0021B660
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterHighPass);
			}
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x0021D280 File Offset: 0x0021B680
		public override void SetOn(Sample sample, float value)
		{
			AudioHighPassFilter audioHighPassFilter = sample.source.GetComponent<AudioHighPassFilter>();
			if (audioHighPassFilter == null)
			{
				audioHighPassFilter = sample.source.gameObject.AddComponent<AudioHighPassFilter>();
			}
			if (this.filterProperty == HighPassFilterProperty.Cutoff)
			{
				audioHighPassFilter.cutoffFrequency = value;
			}
			if (this.filterProperty == HighPassFilterProperty.Resonance)
			{
				audioHighPassFilter.highpassResonanceQ = value;
			}
		}
	}
}
