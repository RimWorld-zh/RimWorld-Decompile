using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B9C RID: 2972
	public class SoundParamTarget_PropertyEcho : SoundParamTarget
	{
		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06004058 RID: 16472 RVA: 0x0021CB6C File Offset: 0x0021AF6C
		public override string Label
		{
			get
			{
				return "EchoFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06004059 RID: 16473 RVA: 0x0021CB98 File Offset: 0x0021AF98
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterEcho);
			}
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x0021CBB8 File Offset: 0x0021AFB8
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
