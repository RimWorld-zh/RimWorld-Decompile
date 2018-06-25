using System;

namespace RimWorld
{
	// Token: 0x02000377 RID: 887
	public class StorytellerCompProperties_SingleMTB : StorytellerCompProperties
	{
		// Token: 0x0400095D RID: 2397
		public IncidentDef incident;

		// Token: 0x0400095E RID: 2398
		public float mtbDays = 13f;

		// Token: 0x06000F4E RID: 3918 RVA: 0x00081AC8 File Offset: 0x0007FEC8
		public StorytellerCompProperties_SingleMTB()
		{
			this.compClass = typeof(StorytellerComp_SingleMTB);
		}
	}
}
