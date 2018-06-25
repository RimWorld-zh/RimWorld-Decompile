using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000251 RID: 593
	public class CompProperties_PsychicDrone : CompProperties
	{
		// Token: 0x040004A4 RID: 1188
		public int droneLevelIncreaseInterval = 150000;

		// Token: 0x06000A8B RID: 2699 RVA: 0x0005FAAC File Offset: 0x0005DEAC
		public CompProperties_PsychicDrone()
		{
			this.compClass = typeof(CompPsychicDrone);
		}
	}
}
