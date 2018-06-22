using System;

namespace Verse
{
	// Token: 0x02000D1D RID: 3357
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		// Token: 0x06004A02 RID: 18946 RVA: 0x0026B770 File Offset: 0x00269B70
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}

		// Token: 0x0400322D RID: 12845
		public float immunityPerDayNotSick = 0f;

		// Token: 0x0400322E RID: 12846
		public float immunityPerDaySick = 0f;

		// Token: 0x0400322F RID: 12847
		public float severityPerDayNotImmune = 0f;

		// Token: 0x04003230 RID: 12848
		public float severityPerDayImmune = 0f;

		// Token: 0x04003231 RID: 12849
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);
	}
}
