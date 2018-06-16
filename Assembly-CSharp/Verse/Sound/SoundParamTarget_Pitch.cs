using System;

namespace Verse.Sound
{
	// Token: 0x02000B96 RID: 2966
	public class SoundParamTarget_Pitch : SoundParamTarget
	{
		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x0600404B RID: 16459 RVA: 0x0021C8C4 File Offset: 0x0021ACC4
		public override string Label
		{
			get
			{
				return "Pitch";
			}
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x0021C8E0 File Offset: 0x0021ACE0
		public override void SetOn(Sample sample, float value)
		{
			float num;
			if (this.pitchType == PitchParamType.Multiply)
			{
				num = value;
			}
			else
			{
				num = (float)Math.Pow(1.05946, (double)value);
			}
			sample.source.pitch = AudioSourceUtility.GetSanitizedPitch(sample.SanitizedPitch * num, "SoundParamTarget_Pitch");
		}

		// Token: 0x04002B1F RID: 11039
		[Description("The scale used for this pitch input.\n\nMultiply means a multiplier for the natural frequency of the sound. 1.0 gives normal sound, 0.5 gives twice as long and one octave down, and 2.0 gives half as long and an octave up.\n\nSemitones sets a number of semitones to offset the sound.")]
		private PitchParamType pitchType = PitchParamType.Multiply;
	}
}
