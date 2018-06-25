using System;

namespace RimWorld
{
	// Token: 0x0200036E RID: 878
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		// Token: 0x04000954 RID: 2388
		public float baseMtbDays = 99999f;

		// Token: 0x06000F37 RID: 3895 RVA: 0x00080F48 File Offset: 0x0007F348
		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}
	}
}
