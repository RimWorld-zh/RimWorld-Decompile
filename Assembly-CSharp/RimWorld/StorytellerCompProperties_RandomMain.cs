using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000373 RID: 883
	public class StorytellerCompProperties_RandomMain : StorytellerCompProperties
	{
		// Token: 0x04000959 RID: 2393
		public float mtbDays;

		// Token: 0x0400095A RID: 2394
		public List<IncidentCategoryEntry> categoryWeights = new List<IncidentCategoryEntry>();

		// Token: 0x0400095B RID: 2395
		public float maxThreatBigIntervalDays = 99999f;

		// Token: 0x06000F44 RID: 3908 RVA: 0x000815C1 File Offset: 0x0007F9C1
		public StorytellerCompProperties_RandomMain()
		{
			this.compClass = typeof(StorytellerComp_RandomMain);
		}
	}
}
