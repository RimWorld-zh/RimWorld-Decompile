using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B96 RID: 2966
	public class SoundParamTarget_PropertyLowPass : SoundParamTarget
	{
		// Token: 0x04002B28 RID: 11048
		private LowPassFilterProperty filterProperty;

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06004055 RID: 16469 RVA: 0x0021D184 File Offset: 0x0021B584
		public override string Label
		{
			get
			{
				return "LowPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06004056 RID: 16470 RVA: 0x0021D1B0 File Offset: 0x0021B5B0
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterLowPass);
			}
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x0021D1D0 File Offset: 0x0021B5D0
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
	}
}
