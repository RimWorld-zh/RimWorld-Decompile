using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B83 RID: 2947
	public class SoundFilterLowPass : SoundFilter
	{
		// Token: 0x0600401F RID: 16415 RVA: 0x0021C35C File Offset: 0x0021A75C
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
