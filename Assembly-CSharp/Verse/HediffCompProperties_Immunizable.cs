using System;

namespace Verse
{
	// Token: 0x02000D20 RID: 3360
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		// Token: 0x04003234 RID: 12852
		public float immunityPerDayNotSick = 0f;

		// Token: 0x04003235 RID: 12853
		public float immunityPerDaySick = 0f;

		// Token: 0x04003236 RID: 12854
		public float severityPerDayNotImmune = 0f;

		// Token: 0x04003237 RID: 12855
		public float severityPerDayImmune = 0f;

		// Token: 0x04003238 RID: 12856
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x06004A05 RID: 18949 RVA: 0x0026BB2C File Offset: 0x00269F2C
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}
	}
}
