using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B82 RID: 2946
	public class SoundFilterReverb : SoundFilter
	{
		// Token: 0x06004027 RID: 16423 RVA: 0x0021CB00 File Offset: 0x0021AF00
		public override void SetupOn(AudioSource source)
		{
			AudioReverbFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioReverbFilter>(source);
			this.baseSetup.ApplyTo(orMakeFilterOn);
		}

		// Token: 0x04002B13 RID: 11027
		[Description("The base setup for this filter.\n\nOnly used if no parameters ever affect this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();
	}
}
