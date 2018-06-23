using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B81 RID: 2945
	public class SoundFilterEcho : SoundFilter
	{
		// Token: 0x04002B0F RID: 11023
		[EditSliderRange(10f, 5000f)]
		[Description("Echo delay in ms. 10 to 5000. Default = 500.")]
		private float delay = 500f;

		// Token: 0x04002B10 RID: 11024
		[EditSliderRange(0f, 1f)]
		[Description("Echo decay per delay. 0 to 1. 1.0 = No decay, 0.0 = total decay (ie simple 1 line delay).")]
		private float decayRatio = 0.5f;

		// Token: 0x04002B11 RID: 11025
		[EditSliderRange(0f, 1f)]
		[Description("The volume of the echo signal to pass to output.")]
		private float wetMix = 1f;

		// Token: 0x04002B12 RID: 11026
		[EditSliderRange(0f, 1f)]
		[Description("The volume of the original signal to pass to output.")]
		private float dryMix = 1f;

		// Token: 0x06004025 RID: 16421 RVA: 0x0021CAA8 File Offset: 0x0021AEA8
		public override void SetupOn(AudioSource source)
		{
			AudioEchoFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioEchoFilter>(source);
			orMakeFilterOn.delay = this.delay;
			orMakeFilterOn.decayRatio = this.decayRatio;
			orMakeFilterOn.wetMix = this.wetMix;
			orMakeFilterOn.dryMix = this.dryMix;
		}
	}
}
