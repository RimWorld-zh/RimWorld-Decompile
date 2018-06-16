using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B86 RID: 2950
	public class SoundFilterReverb : SoundFilter
	{
		// Token: 0x06004023 RID: 16419 RVA: 0x0021C390 File Offset: 0x0021A790
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
