using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B85 RID: 2949
	public class SoundFilterEcho : SoundFilter
	{
		// Token: 0x06004023 RID: 16419 RVA: 0x0021C40C File Offset: 0x0021A80C
		public override void SetupOn(AudioSource source)
		{
			AudioEchoFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioEchoFilter>(source);
			orMakeFilterOn.delay = this.delay;
			orMakeFilterOn.decayRatio = this.decayRatio;
			orMakeFilterOn.wetMix = this.wetMix;
			orMakeFilterOn.dryMix = this.dryMix;
		}

		// Token: 0x04002B0A RID: 11018
		[EditSliderRange(10f, 5000f)]
		[Description("Echo delay in ms. 10 to 5000. Default = 500.")]
		private float delay = 500f;

		// Token: 0x04002B0B RID: 11019
		[EditSliderRange(0f, 1f)]
		[Description("Echo decay per delay. 0 to 1. 1.0 = No decay, 0.0 = total decay (ie simple 1 line delay).")]
		private float decayRatio = 0.5f;

		// Token: 0x04002B0C RID: 11020
		[EditSliderRange(0f, 1f)]
		[Description("The volume of the echo signal to pass to output.")]
		private float wetMix = 1f;

		// Token: 0x04002B0D RID: 11021
		[EditSliderRange(0f, 1f)]
		[Description("The volume of the original signal to pass to output.")]
		private float dryMix = 1f;
	}
}
