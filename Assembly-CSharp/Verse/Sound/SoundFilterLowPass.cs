using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B83 RID: 2947
	public class SoundFilterLowPass : SoundFilter
	{
		// Token: 0x0600401D RID: 16413 RVA: 0x0021C288 File Offset: 0x0021A688
		public override void SetupOn(AudioSource source)
		{
			AudioLowPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioLowPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.lowpassResonanceQ = this.lowpassResonaceQ;
		}

		// Token: 0x04002B06 RID: 11014
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies above this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x04002B07 RID: 11015
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float lowpassResonaceQ = 1f;
	}
}
