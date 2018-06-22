using System;

namespace RimWorld
{
	// Token: 0x02000375 RID: 885
	public class StorytellerCompProperties_SingleMTB : StorytellerCompProperties
	{
		// Token: 0x06000F4B RID: 3915 RVA: 0x00081968 File Offset: 0x0007FD68
		public StorytellerCompProperties_SingleMTB()
		{
			this.compClass = typeof(StorytellerComp_SingleMTB);
		}

		// Token: 0x0400095A RID: 2394
		public IncidentDef incident;

		// Token: 0x0400095B RID: 2395
		public float mtbDays = 13f;
	}
}
