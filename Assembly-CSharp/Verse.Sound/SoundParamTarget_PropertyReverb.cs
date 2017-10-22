using System;
using UnityEngine;

namespace Verse.Sound
{
	public class SoundParamTarget_PropertyReverb : SoundParamTarget
	{
		[Description("The base setup for the reverb.\n\nOnly used if no parameters are touching this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();

		[Description("The interpolation target setup for this filter.\n\nWhen the interpolant parameter is at 1, these settings will be active.")]
		private ReverbSetup targetSetup = new ReverbSetup();

		public override string Label
		{
			get
			{
				return "ReverbFilter-interpolant";
			}
		}

		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterReverb);
			}
		}

		public override void SetOn(Sample sample, float value)
		{
			AudioReverbFilter audioReverbFilter = ((Component)sample.source).GetComponent<AudioReverbFilter>();
			if ((UnityEngine.Object)audioReverbFilter == (UnityEngine.Object)null)
			{
				audioReverbFilter = sample.source.gameObject.AddComponent<AudioReverbFilter>();
			}
			ReverbSetup reverbSetup;
			if (value < 0.0010000000474974513)
			{
				reverbSetup = this.baseSetup;
			}
			reverbSetup = ((!(value > 0.99900001287460327)) ? ReverbSetup.Lerp(this.baseSetup, this.targetSetup, value) : this.targetSetup);
			reverbSetup.ApplyTo(audioReverbFilter);
		}
	}
}
