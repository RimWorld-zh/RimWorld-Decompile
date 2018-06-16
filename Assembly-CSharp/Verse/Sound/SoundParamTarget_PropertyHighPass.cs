using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B9A RID: 2970
	public class SoundParamTarget_PropertyHighPass : SoundParamTarget
	{
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06004052 RID: 16466 RVA: 0x0021C9E8 File Offset: 0x0021ADE8
		public override string Label
		{
			get
			{
				return "HighPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06004053 RID: 16467 RVA: 0x0021CA14 File Offset: 0x0021AE14
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterHighPass);
			}
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x0021CA34 File Offset: 0x0021AE34
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

		// Token: 0x04002B27 RID: 11047
		private HighPassFilterProperty filterProperty;
	}
}
