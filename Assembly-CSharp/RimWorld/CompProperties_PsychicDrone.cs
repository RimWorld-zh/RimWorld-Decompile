using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000251 RID: 593
	public class CompProperties_PsychicDrone : CompProperties
	{
		// Token: 0x040004A6 RID: 1190
		public int droneLevelIncreaseInterval = 150000;

		// Token: 0x06000A8A RID: 2698 RVA: 0x0005FAA8 File Offset: 0x0005DEA8
		public CompProperties_PsychicDrone()
		{
			this.compClass = typeof(CompPsychicDrone);
		}
	}
}
