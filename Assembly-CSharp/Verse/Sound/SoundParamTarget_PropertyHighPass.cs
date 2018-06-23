using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B96 RID: 2966
	public class SoundParamTarget_PropertyHighPass : SoundParamTarget
	{
		// Token: 0x04002B2C RID: 11052
		private HighPassFilterProperty filterProperty;

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06004056 RID: 16470 RVA: 0x0021D158 File Offset: 0x0021B558
		public override string Label
		{
			get
			{
				return "HighPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06004057 RID: 16471 RVA: 0x0021D184 File Offset: 0x0021B584
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterHighPass);
			}
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x0021D1A4 File Offset: 0x0021B5A4
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
