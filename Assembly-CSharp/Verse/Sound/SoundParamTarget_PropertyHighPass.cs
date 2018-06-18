using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B9A RID: 2970
	public class SoundParamTarget_PropertyHighPass : SoundParamTarget
	{
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06004054 RID: 16468 RVA: 0x0021CABC File Offset: 0x0021AEBC
		public override string Label
		{
			get
			{
				return "HighPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06004055 RID: 16469 RVA: 0x0021CAE8 File Offset: 0x0021AEE8
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterHighPass);
			}
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x0021CB08 File Offset: 0x0021AF08
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
