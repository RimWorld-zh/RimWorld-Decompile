using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000371 RID: 881
	public class StorytellerCompProperties_RandomMain : StorytellerCompProperties
	{
		// Token: 0x06000F41 RID: 3905 RVA: 0x00081275 File Offset: 0x0007F675
		public StorytellerCompProperties_RandomMain()
		{
			this.compClass = typeof(StorytellerComp_RandomMain);
		}

		// Token: 0x04000954 RID: 2388
		public float mtbDays;

		// Token: 0x04000955 RID: 2389
		public List<IncidentCategoryEntry> categoryWeights = new List<IncidentCategoryEntry>();

		// Token: 0x04000956 RID: 2390
		public float maxThreatBigIntervalDays = 99999f;
	}
}
