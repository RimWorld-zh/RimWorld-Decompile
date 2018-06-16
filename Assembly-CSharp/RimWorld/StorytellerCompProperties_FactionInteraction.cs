using System;

namespace RimWorld
{
	// Token: 0x0200036C RID: 876
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		// Token: 0x06000F34 RID: 3892 RVA: 0x00080BFC File Offset: 0x0007EFFC
		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}

		// Token: 0x0400094F RID: 2383
		public float baseMtbDays = 99999f;
	}
}
