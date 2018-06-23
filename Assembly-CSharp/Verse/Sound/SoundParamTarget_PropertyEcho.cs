using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B98 RID: 2968
	public class SoundParamTarget_PropertyEcho : SoundParamTarget
	{
		// Token: 0x04002B32 RID: 11058
		private EchoFilterProperty filterProperty;

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x0600405A RID: 16474 RVA: 0x0021D208 File Offset: 0x0021B608
		public override string Label
		{
			get
			{
				return "EchoFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x0021D234 File Offset: 0x0021B634
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterEcho);
			}
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0021D254 File Offset: 0x0021B654
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
