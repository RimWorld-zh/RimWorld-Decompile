using System;

namespace RimWorld
{
	// Token: 0x02000377 RID: 887
	public class StorytellerCompProperties_SingleMTB : StorytellerCompProperties
	{
		// Token: 0x0400095A RID: 2394
		public IncidentDef incident;

		// Token: 0x0400095B RID: 2395
		public float mtbDays = 13f;

		// Token: 0x06000F4F RID: 3919 RVA: 0x00081AB8 File Offset: 0x0007FEB8
		public StorytellerCompProperties_SingleMTB()
		{
			this.compClass = typeof(StorytellerComp_SingleMTB);
		}
	}
}
