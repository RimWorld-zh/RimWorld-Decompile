using System;

namespace RimWorld
{
	// Token: 0x0200036C RID: 876
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		// Token: 0x04000951 RID: 2385
		public float baseMtbDays = 99999f;

		// Token: 0x06000F34 RID: 3892 RVA: 0x00080DE8 File Offset: 0x0007F1E8
		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}
	}
}
