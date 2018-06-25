using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B82 RID: 2946
	public class SoundFilterHighPass : SoundFilter
	{
		// Token: 0x04002B0D RID: 11021
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies below this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x04002B0E RID: 11022
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float highpassResonanceQ = 1f;

		// Token: 0x06004026 RID: 16422 RVA: 0x0021CB20 File Offset: 0x0021AF20
		public override void SetupOn(AudioSource source)
		{
			AudioHighPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioHighPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.highpassResonanceQ = this.highpassResonanceQ;
		}
	}
}
