using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024F RID: 591
	public class CompProperties_PsychicDrone : CompProperties
	{
		// Token: 0x040004A4 RID: 1188
		public int droneLevelIncreaseInterval = 150000;

		// Token: 0x06000A87 RID: 2695 RVA: 0x0005F95C File Offset: 0x0005DD5C
		public CompProperties_PsychicDrone()
		{
			this.compClass = typeof(CompPsychicDrone);
		}
	}
}
