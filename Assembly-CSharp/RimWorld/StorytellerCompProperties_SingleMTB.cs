using System;

namespace RimWorld
{
	// Token: 0x02000375 RID: 885
	public class StorytellerCompProperties_SingleMTB : StorytellerCompProperties
	{
		// Token: 0x06000F4B RID: 3915 RVA: 0x0008177C File Offset: 0x0007FB7C
		public StorytellerCompProperties_SingleMTB()
		{
			this.compClass = typeof(StorytellerComp_SingleMTB);
		}

		// Token: 0x04000958 RID: 2392
		public IncidentDef incident;

		// Token: 0x04000959 RID: 2393
		public float mtbDays = 13f;
	}
}
