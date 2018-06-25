using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B91 RID: 2961
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		// Token: 0x04002B27 RID: 11047
		public TimeType timeType = TimeType.Ticks;

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06004048 RID: 16456 RVA: 0x0021D310 File Offset: 0x0021B710
		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0021D32C File Offset: 0x0021B72C
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
