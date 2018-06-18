using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B86 RID: 2950
	public class SoundFilterReverb : SoundFilter
	{
		// Token: 0x06004025 RID: 16421 RVA: 0x0021C464 File Offset: 0x0021A864
		public override void SetupOn(AudioSource source)
		{
			AudioReverbFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioReverbFilter>(source);
			this.baseSetup.ApplyTo(orMakeFilterOn);
		}

		// Token: 0x04002B0E RID: 11022
		[Description("The base setup for this filter.\n\nOnly used if no parameters ever affect this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();
	}
}
