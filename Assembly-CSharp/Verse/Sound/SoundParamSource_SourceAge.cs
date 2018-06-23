using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B8E RID: 2958
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		// Token: 0x04002B20 RID: 11040
		public TimeType timeType = TimeType.Ticks;

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x0021CF54 File Offset: 0x0021B354
		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x0021CF70 File Offset: 0x0021B370
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
