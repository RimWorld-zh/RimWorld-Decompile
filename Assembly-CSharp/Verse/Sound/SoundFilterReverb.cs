using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B85 RID: 2949
	public class SoundFilterReverb : SoundFilter
	{
		// Token: 0x04002B1A RID: 11034
		[Description("The base setup for this filter.\n\nOnly used if no parameters ever affect this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();

		// Token: 0x0600402A RID: 16426 RVA: 0x0021CEBC File Offset: 0x0021B2BC
		public override void SetupOn(AudioSource source)
		{
			AudioReverbFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioReverbFilter>(source);
			this.baseSetup.ApplyTo(orMakeFilterOn);
		}
	}
}
