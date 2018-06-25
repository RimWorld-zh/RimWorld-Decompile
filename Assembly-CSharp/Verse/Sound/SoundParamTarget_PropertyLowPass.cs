using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B97 RID: 2967
	public class SoundParamTarget_PropertyLowPass : SoundParamTarget
	{
		// Token: 0x04002B2F RID: 11055
		private LowPassFilterProperty filterProperty;

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06004055 RID: 16469 RVA: 0x0021D464 File Offset: 0x0021B864
		public override string Label
		{
			get
			{
				return "LowPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06004056 RID: 16470 RVA: 0x0021D490 File Offset: 0x0021B890
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterLowPass);
			}
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x0021D4B0 File Offset: 0x0021B8B0
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
