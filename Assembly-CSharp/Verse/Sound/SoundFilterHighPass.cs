using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B84 RID: 2948
	public class SoundFilterHighPass : SoundFilter
	{
		// Token: 0x0600401F RID: 16415 RVA: 0x0021C2D4 File Offset: 0x0021A6D4
		public override void SetupOn(AudioSource source)
		{
			AudioHighPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioHighPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.highpassResonanceQ = this.highpassResonanceQ;
		}

		// Token: 0x04002B08 RID: 11016
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies below this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x04002B09 RID: 11017
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float highpassResonanceQ = 1f;
	}
}
