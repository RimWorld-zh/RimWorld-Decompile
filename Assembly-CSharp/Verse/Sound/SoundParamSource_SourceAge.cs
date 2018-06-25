using System;
using UnityEngine;

namespace Verse.Sound
{
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		public TimeType timeType = TimeType.Ticks;

		public SoundParamSource_SourceAge()
		{
		}

		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		public override float ValueFor(Sample samp)
		{
			float result;
			if (this.timeType == TimeType.RealtimeSeconds)
			{
				result = Time.realtimeSinceStartup - samp.ParentStartRealTime;
			}
			else
			{
				if (this.timeType == TimeType.Ticks)
				{
					if (Find.TickManager != null)
					{
						return (float)Find.TickManager.TicksGame - samp.ParentStartTick;
					}
				}
				result = 0f;
			}
			return result;
		}
	}
}
