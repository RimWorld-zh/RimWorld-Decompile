using System;

namespace RimWorld
{
	// Token: 0x0200036E RID: 878
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		// Token: 0x04000951 RID: 2385
		public float baseMtbDays = 99999f;

		// Token: 0x06000F38 RID: 3896 RVA: 0x00080F38 File Offset: 0x0007F338
		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}
	}
}
