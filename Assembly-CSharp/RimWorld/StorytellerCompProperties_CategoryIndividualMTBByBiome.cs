using System;

namespace RimWorld
{
	// Token: 0x02000362 RID: 866
	public class StorytellerCompProperties_CategoryIndividualMTBByBiome : StorytellerCompProperties
	{
		// Token: 0x06000F1A RID: 3866 RVA: 0x0007FA76 File Offset: 0x0007DE76
		public StorytellerCompProperties_CategoryIndividualMTBByBiome()
		{
			this.compClass = typeof(StorytellerComp_CategoryIndividualMTBByBiome);
		}

		// Token: 0x04000945 RID: 2373
		public IncidentCategoryDef category;

		// Token: 0x04000946 RID: 2374
		public bool applyCaravanVisibility;
	}
}
