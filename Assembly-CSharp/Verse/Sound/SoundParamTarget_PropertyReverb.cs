using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B9C RID: 2972
	public class SoundParamTarget_PropertyReverb : SoundParamTarget
	{
		// Token: 0x04002B3A RID: 11066
		[Description("The base setup for the reverb.\n\nOnly used if no parameters are touching this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();

		// Token: 0x04002B3B RID: 11067
		[Description("The interpolation target setup for this filter.\n\nWhen the interpolant parameter is at 1, these settings will be active.")]
		private ReverbSetup targetSetup = new ReverbSetup();

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06004061 RID: 16481 RVA: 0x0021D6B0 File Offset: 0x0021BAB0
		public override string Label
		{
			get
			{
				return "ReverbFilter-interpolant";
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06004062 RID: 16482 RVA: 0x0021D6CC File Offset: 0x0021BACC
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterReverb);
			}
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x0021D6EC File Offset: 0x0021BAEC
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
