using System;

namespace RimWorld
{
	// Token: 0x02000362 RID: 866
	public class StorytellerCompProperties_AllyAssistance : StorytellerCompProperties
	{
		// Token: 0x04000949 RID: 2377
		public float baseMtbDays = 99999f;

		// Token: 0x06000F18 RID: 3864 RVA: 0x0007F9A6 File Offset: 0x0007DDA6
		public StorytellerCompProperties_AllyAssistance()
		{
			this.compClass = typeof(StorytellerComp_AllyAssistance);
		}
	}
}
