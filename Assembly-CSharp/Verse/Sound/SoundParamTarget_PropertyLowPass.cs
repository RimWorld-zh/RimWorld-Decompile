using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B94 RID: 2964
	public class SoundParamTarget_PropertyLowPass : SoundParamTarget
	{
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06004052 RID: 16466 RVA: 0x0021D0A8 File Offset: 0x0021B4A8
		public override string Label
		{
			get
			{
				return "LowPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06004053 RID: 16467 RVA: 0x0021D0D4 File Offset: 0x0021B4D4
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterLowPass);
			}
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x0021D0F4 File Offset: 0x0021B4F4
		public override void SetOn(Sample sample, float value)
		{
			AudioLowPassFilter audioLowPassFilter = sample.source.GetComponent<AudioLowPassFilter>();
			if (audioLowPassFilter == null)
			{
				audioLowPassFilter = sample.source.gameObject.AddComponent<AudioLowPassFilter>();
			}
			if (this.filterProperty == LowPassFilterProperty.Cutoff)
			{
				audioLowPassFilter.cutoffFrequency = value;
			}
			if (this.filterProperty == LowPassFilterProperty.Resonance)
			{
				audioLowPassFilter.lowpassResonanceQ = value;
			}
		}

		// Token: 0x04002B28 RID: 11048
		private LowPassFilterProperty filterProperty;
	}
}
