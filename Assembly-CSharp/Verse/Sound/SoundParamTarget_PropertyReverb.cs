using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B99 RID: 2969
	public class SoundParamTarget_PropertyReverb : SoundParamTarget
	{
		// Token: 0x04002B33 RID: 11059
		[Description("The base setup for the reverb.\n\nOnly used if no parameters are touching this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();

		// Token: 0x04002B34 RID: 11060
		[Description("The interpolation target setup for this filter.\n\nWhen the interpolant parameter is at 1, these settings will be active.")]
		private ReverbSetup targetSetup = new ReverbSetup();

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x0600405E RID: 16478 RVA: 0x0021D2F4 File Offset: 0x0021B6F4
		public override string Label
		{
			get
			{
				return "ReverbFilter-interpolant";
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x0600405F RID: 16479 RVA: 0x0021D310 File Offset: 0x0021B710
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterReverb);
			}
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x0021D330 File Offset: 0x0021B730
		public override void SetOn(Sample sample, float value)
		{
			AudioReverbFilter audioReverbFilter = sample.source.GetComponent<AudioReverbFilter>();
			if (audioReverbFilter == null)
			{
				audioReverbFilter = sample.source.gameObject.AddComponent<AudioReverbFilter>();
			}
			ReverbSetup reverbSetup;
			if (value < 0.001f)
			{
				reverbSetup = this.baseSetup;
			}
			if (value > 0.999f)
			{
				reverbSetup = this.targetSetup;
			}
			else
			{
				reverbSetup = ReverbSetup.Lerp(this.baseSetup, this.targetSetup, value);
			}
			reverbSetup.ApplyTo(audioReverbFilter);
		}
	}
}
