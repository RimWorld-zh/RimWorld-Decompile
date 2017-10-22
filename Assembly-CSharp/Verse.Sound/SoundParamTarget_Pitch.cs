using System;

namespace Verse.Sound
{
	public class SoundParamTarget_Pitch : SoundParamTarget
	{
		[Description("The scale used for this pitch input.\n\nMultiply means a multiplier for the natural frequency of the sound. 1.0 gives normal sound, 0.5 gives twice as long and one octave down, and 2.0 gives half as long and an octave up.\n\nSemitones sets a number of semitones to offset the sound.")]
		private PitchParamType pitchType = PitchParamType.Multiply;

		public override string Label
		{
			get
			{
				return "Pitch";
			}
		}

		public override void SetOn(Sample sample, float value)
		{
			float num = (this.pitchType != 0) ? ((float)Math.Pow(1.05946, (double)value)) : value;
			sample.source.pitch = AudioSourceUtility.GetSanitizedPitch(sample.SanitizedPitch * num, "SoundParamTarget_Pitch");
		}
	}
}
