using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024F RID: 591
	public class CompProperties_PsychicDrone : CompProperties
	{
		// Token: 0x06000A89 RID: 2697 RVA: 0x0005F900 File Offset: 0x0005DD00
		public CompProperties_PsychicDrone()
		{
			this.compClass = typeof(CompPsychicDrone);
		}

		// Token: 0x040004A6 RID: 1190
		public int droneLevelIncreaseInterval = 150000;
	}
}
