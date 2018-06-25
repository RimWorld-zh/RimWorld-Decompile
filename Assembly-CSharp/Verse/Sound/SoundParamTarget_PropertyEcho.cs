using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B9A RID: 2970
	public class SoundParamTarget_PropertyEcho : SoundParamTarget
	{
		// Token: 0x04002B32 RID: 11058
		private EchoFilterProperty filterProperty;

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x0600405D RID: 16477 RVA: 0x0021D2E4 File Offset: 0x0021B6E4
		public override string Label
		{
			get
			{
				return "EchoFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x0600405E RID: 16478 RVA: 0x0021D310 File Offset: 0x0021B710
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterEcho);
			}
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x0021D330 File Offset: 0x0021B730
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
	}
}
