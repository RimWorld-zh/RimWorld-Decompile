using System;

namespace Verse
{
	// Token: 0x02000D10 RID: 3344
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		// Token: 0x060049AE RID: 18862 RVA: 0x002688DC File Offset: 0x00266CDC
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}

		// Token: 0x040031F3 RID: 12787
		public float severityPerDayGrowing = 0f;

		// Token: 0x040031F4 RID: 12788
		public float severityPerDayRemission = 0f;

		// Token: 0x040031F5 RID: 12789
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x040031F6 RID: 12790
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);
	}
}
