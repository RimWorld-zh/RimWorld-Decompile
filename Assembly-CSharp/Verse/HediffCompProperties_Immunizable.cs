using System;

namespace Verse
{
	// Token: 0x02000D20 RID: 3360
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		// Token: 0x060049F1 RID: 18929 RVA: 0x0026A33C File Offset: 0x0026873C
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}

		// Token: 0x04003222 RID: 12834
		public float immunityPerDayNotSick = 0f;

		// Token: 0x04003223 RID: 12835
		public float immunityPerDaySick = 0f;

		// Token: 0x04003224 RID: 12836
		public float severityPerDayNotImmune = 0f;

		// Token: 0x04003225 RID: 12837
		public float severityPerDayImmune = 0f;

		// Token: 0x04003226 RID: 12838
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);
	}
}
