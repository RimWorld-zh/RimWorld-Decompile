using System;

namespace RimWorld
{
	// Token: 0x02000362 RID: 866
	public class StorytellerCompProperties_AllyAssistance : StorytellerCompProperties
	{
		// Token: 0x04000946 RID: 2374
		public float baseMtbDays = 99999f;

		// Token: 0x06000F19 RID: 3865 RVA: 0x0007F996 File Offset: 0x0007DD96
		public StorytellerCompProperties_AllyAssistance()
		{
			this.compClass = typeof(StorytellerComp_AllyAssistance);
		}
	}
}
