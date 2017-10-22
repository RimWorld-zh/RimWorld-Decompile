using UnityEngine;

namespace Verse.Sound
{
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		public TimeType timeType = TimeType.Ticks;

		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		public override float ValueFor(Sample samp)
		{
			return (float)((this.timeType != TimeType.RealtimeSeconds) ? ((this.timeType != 0 || Find.TickManager == null) ? 0.0 : ((float)Find.TickManager.TicksGame - samp.ParentStartTick)) : (Time.realtimeSinceStartup - samp.ParentStartRealTime));
		}
	}
}
