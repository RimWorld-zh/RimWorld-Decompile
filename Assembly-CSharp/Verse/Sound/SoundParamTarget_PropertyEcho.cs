using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B9C RID: 2972
	public class SoundParamTarget_PropertyEcho : SoundParamTarget
	{
		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06004056 RID: 16470 RVA: 0x0021CA98 File Offset: 0x0021AE98
		public override string Label
		{
			get
			{
				return "EchoFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06004057 RID: 16471 RVA: 0x0021CAC4 File Offset: 0x0021AEC4
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterEcho);
			}
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x0021CAE4 File Offset: 0x0021AEE4
		public override void SetOn(Sample sample, float value)
		{
			AudioEchoFilter audioEchoFilter = sample.source.GetComponent<AudioEchoFilter>();
			if (audioEchoFilter == null)
			{
				audioEchoFilter = sample.source.gameObject.AddComponent<AudioEchoFilter>();
			}
			if (this.filterProperty == EchoFilterProperty.Delay)
			{
				audioEchoFilter.delay = value;
			}
			if (this.filterProperty == EchoFilterProperty.DecayRatio)
			{
				audioEchoFilter.decayRatio = value;
			}
			if (this.filterProperty == EchoFilterProperty.WetMix)
			{
				audioEchoFilter.wetMix = value;
			}
			if (this.filterProperty == EchoFilterProperty.DryMix)
			{
				audioEchoFilter.dryMix = value;
			}
		}

		// Token: 0x04002B2D RID: 11053
		private EchoFilterProperty filterProperty;
	}
}
