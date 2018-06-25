using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B83 RID: 2947
	public class SoundFilterHighPass : SoundFilter
	{
		// Token: 0x04002B14 RID: 11028
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies below this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x04002B15 RID: 11029
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float highpassResonanceQ = 1f;

		// Token: 0x06004026 RID: 16422 RVA: 0x0021CE00 File Offset: 0x0021B200
		public override void SetupOn(AudioSource source)
		{
			AudioHighPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioHighPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.highpassResonanceQ = this.highpassResonanceQ;
		}
	}
}
