using System;

namespace RimWorld
{
	// Token: 0x02000360 RID: 864
	public class StorytellerCompProperties_AllyAssistance : StorytellerCompProperties
	{
		// Token: 0x04000946 RID: 2374
		public float baseMtbDays = 99999f;

		// Token: 0x06000F15 RID: 3861 RVA: 0x0007F846 File Offset: 0x0007DC46
		public StorytellerCompProperties_AllyAssistance()
		{
			this.compClass = typeof(StorytellerComp_AllyAssistance);
		}
	}
}
