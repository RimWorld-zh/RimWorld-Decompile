using System;

namespace Verse
{
	// Token: 0x02000D21 RID: 3361
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		// Token: 0x060049F3 RID: 18931 RVA: 0x0026A364 File Offset: 0x00268764
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}

		// Token: 0x04003224 RID: 12836
		public float immunityPerDayNotSick = 0f;

		// Token: 0x04003225 RID: 12837
		public float immunityPerDaySick = 0f;

		// Token: 0x04003226 RID: 12838
		public float severityPerDayNotImmune = 0f;

		// Token: 0x04003227 RID: 12839
		public float severityPerDayImmune = 0f;

		// Token: 0x04003228 RID: 12840
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);
	}
}
