using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000366 RID: 870
	public class StorytellerCompProperties_CategoryMTB : StorytellerCompProperties
	{
		// Token: 0x0400094C RID: 2380
		public float mtbDays = -1f;

		// Token: 0x0400094D RID: 2381
		public SimpleCurve mtbDaysFactorByDaysPassedCurve = null;

		// Token: 0x0400094E RID: 2382
		public IncidentCategoryDef category;

		// Token: 0x06000F22 RID: 3874 RVA: 0x0008006C File Offset: 0x0007E46C
		public StorytellerCompProperties_CategoryMTB()
		{
			this.compClass = typeof(StorytellerComp_CategoryMTB);
		}
	}
}
