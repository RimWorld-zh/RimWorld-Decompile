using UnityEngine;
using Verse.Noise;

namespace Verse.Sound
{
	public class SoundParamSource_Perlin : SoundParamSource
	{
		[Description("The type of time on which this perlin randomizer will work. If you use Ticks, it will freeze when paused and speed up in fast forward.")]
		public TimeType timeType;

		[Description("The frequency of the perlin output. The input time is multiplied by this amount.")]
		public float perlinFrequency = 1f;

		[Description("Whether to synchronize the Perlin output across different samples. If set to desync, each playing sample will get a separate Perlin output.")]
		public PerlinMappingSyncType syncType;

		private static Perlin perlin = new Perlin(0.0099999997764825821, 2.0, 0.5, 4, Rand.Range(0, 2147483647), QualityMode.Medium);

		public override string Label
		{
			get
			{
				return "Perlin noise";
			}
		}

		public override float ValueFor(Sample samp)
		{
			float num = (float)((this.syncType != 0) ? ((float)(samp.GetHashCode() % 100)) : (samp.ParentHashCode % 100.0));
			if (this.timeType == TimeType.Ticks && Current.ProgramState == ProgramState.Playing)
			{
				float num2 = (this.syncType != 0) ? ((float)(Find.TickManager.TicksGame - samp.startTick)) : ((float)Find.TickManager.TicksGame - samp.ParentStartTick);
				num2 = (float)(num2 / 60.0);
				num += num2;
			}
			else
			{
				float num3 = (this.syncType != 0) ? (Time.realtimeSinceStartup - samp.startRealTime) : (Time.realtimeSinceStartup - samp.ParentStartRealTime);
				num += num3;
			}
			num *= this.perlinFrequency;
			float num4 = (float)SoundParamSource_Perlin.perlin.GetValue((double)num, 0.0, 0.0);
			num4 = (float)(num4 * 2.0);
			num4 = (float)(num4 + 1.0);
			return (float)(num4 / 2.0);
		}
	}
}
