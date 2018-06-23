using System;

namespace RimWorld
{
	// Token: 0x02000362 RID: 866
	public class StorytellerCompProperties_CategoryIndividualMTBByBiome : StorytellerCompProperties
	{
		// Token: 0x04000947 RID: 2375
		public IncidentCategoryDef category;

		// Token: 0x04000948 RID: 2376
		public bool applyCaravanVisibility;

		// Token: 0x06000F1A RID: 3866 RVA: 0x0007FC62 File Offset: 0x0007E062
		public StorytellerCompProperties_CategoryIndividualMTBByBiome()
		{
			this.compClass = typeof(StorytellerComp_CategoryIndividualMTBByBiome);
		}
	}
}
