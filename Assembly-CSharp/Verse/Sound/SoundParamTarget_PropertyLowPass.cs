using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B98 RID: 2968
	public class SoundParamTarget_PropertyLowPass : SoundParamTarget
	{
		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x0021C938 File Offset: 0x0021AD38
		public override string Label
		{
			get
			{
				return "LowPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x0600404F RID: 16463 RVA: 0x0021C964 File Offset: 0x0021AD64
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterLowPass);
			}
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0021C984 File Offset: 0x0021AD84
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

		// Token: 0x04002B23 RID: 11043
		private LowPassFilterProperty filterProperty;
	}
}
