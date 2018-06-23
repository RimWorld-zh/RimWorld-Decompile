using System;

namespace Verse.Sound
{
	// Token: 0x02000B92 RID: 2962
	public class SoundParamTarget_Pitch : SoundParamTarget
	{
		// Token: 0x04002B24 RID: 11044
		[Description("The scale used for this pitch input.\n\nMultiply means a multiplier for the natural frequency of the sound. 1.0 gives normal sound, 0.5 gives twice as long and one octave down, and 2.0 gives half as long and an octave up.\n\nSemitones sets a number of semitones to offset the sound.")]
		private PitchParamType pitchType = PitchParamType.Multiply;

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x0600404F RID: 16463 RVA: 0x0021D034 File Offset: 0x0021B434
		public override string Label
		{
			get
			{
				return "Pitch";
			}
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0021D050 File Offset: 0x0021B450
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
	}
}
