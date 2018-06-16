using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B92 RID: 2962
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06004041 RID: 16449 RVA: 0x0021C7E4 File Offset: 0x0021ABE4
		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x0021C800 File Offset: 0x0021AC00
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

		// Token: 0x04002B1B RID: 11035
		public TimeType timeType = TimeType.Ticks;
	}
}
