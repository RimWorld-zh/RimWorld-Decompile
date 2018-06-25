using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B90 RID: 2960
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		// Token: 0x04002B20 RID: 11040
		public TimeType timeType = TimeType.Ticks;

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06004048 RID: 16456 RVA: 0x0021D030 File Offset: 0x0021B430
		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0021D04C File Offset: 0x0021B44C
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
