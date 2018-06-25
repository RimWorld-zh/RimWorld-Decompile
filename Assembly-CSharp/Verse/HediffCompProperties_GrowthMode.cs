using System;

namespace Verse
{
	// Token: 0x02000D0F RID: 3343
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		// Token: 0x040031FE RID: 12798
		public float severityPerDayGrowing = 0f;

		// Token: 0x040031FF RID: 12799
		public float severityPerDayRemission = 0f;

		// Token: 0x04003200 RID: 12800
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x04003201 RID: 12801
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x060049C2 RID: 18882 RVA: 0x00269DEC File Offset: 0x002681EC
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}
	}
}
