using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000366 RID: 870
	public class StorytellerCompProperties_CategoryMTB : StorytellerCompProperties
	{
		// Token: 0x04000949 RID: 2377
		public float mtbDays = -1f;

		// Token: 0x0400094A RID: 2378
		public SimpleCurve mtbDaysFactorByDaysPassedCurve = null;

		// Token: 0x0400094B RID: 2379
		public IncidentCategoryDef category;

		// Token: 0x06000F23 RID: 3875 RVA: 0x0008005C File Offset: 0x0007E45C
		public StorytellerCompProperties_CategoryMTB()
		{
			this.compClass = typeof(StorytellerComp_CategoryMTB);
		}
	}
}
