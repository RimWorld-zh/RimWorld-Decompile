using System;

namespace Verse
{
	// Token: 0x02000D10 RID: 3344
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		// Token: 0x04003205 RID: 12805
		public float severityPerDayGrowing = 0f;

		// Token: 0x04003206 RID: 12806
		public float severityPerDayRemission = 0f;

		// Token: 0x04003207 RID: 12807
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x04003208 RID: 12808
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x060049C2 RID: 18882 RVA: 0x0026A0CC File Offset: 0x002684CC
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}
	}
}
