using System;

namespace RimWorld
{
	// Token: 0x02000364 RID: 868
	public class StorytellerCompProperties_CategoryIndividualMTBByBiome : StorytellerCompProperties
	{
		// Token: 0x0400094A RID: 2378
		public IncidentCategoryDef category;

		// Token: 0x0400094B RID: 2379
		public bool applyCaravanVisibility;

		// Token: 0x06000F1D RID: 3869 RVA: 0x0007FDC2 File Offset: 0x0007E1C2
		public StorytellerCompProperties_CategoryIndividualMTBByBiome()
		{
			this.compClass = typeof(StorytellerComp_CategoryIndividualMTBByBiome);
		}
	}
}
