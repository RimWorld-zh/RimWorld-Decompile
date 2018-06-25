using System;

namespace Verse
{
	// Token: 0x02000D1F RID: 3359
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
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

		// Token: 0x06004A05 RID: 18949 RVA: 0x0026B84C File Offset: 0x00269C4C
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}
	}
}
