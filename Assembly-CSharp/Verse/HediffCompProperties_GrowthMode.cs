using System;

namespace Verse
{
	// Token: 0x02000D11 RID: 3345
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		// Token: 0x060049B0 RID: 18864 RVA: 0x00268904 File Offset: 0x00266D04
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}

		// Token: 0x040031F5 RID: 12789
		public float severityPerDayGrowing = 0f;

		// Token: 0x040031F6 RID: 12790
		public float severityPerDayRemission = 0f;

		// Token: 0x040031F7 RID: 12791
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x040031F8 RID: 12792
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);
	}
}
