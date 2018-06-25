using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B81 RID: 2945
	public class SoundFilterLowPass : SoundFilter
	{
		// Token: 0x04002B0B RID: 11019
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies above this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x04002B0C RID: 11020
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float lowpassResonaceQ = 1f;

		// Token: 0x06004024 RID: 16420 RVA: 0x0021CAD4 File Offset: 0x0021AED4
		public override void SetupOn(AudioSource source)
		{
			AudioLowPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioLowPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.lowpassResonanceQ = this.lowpassResonaceQ;
		}
	}
}
