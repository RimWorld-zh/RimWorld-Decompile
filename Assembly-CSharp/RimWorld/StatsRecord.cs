using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200035A RID: 858
	public class StatsRecord : IExposable
	{
		// Token: 0x04000925 RID: 2341
		public int numRaidsEnemy = 0;

		// Token: 0x04000926 RID: 2342
		public int numThreatBigs = 0;

		// Token: 0x04000927 RID: 2343
		public int colonistsKilled = 0;

		// Token: 0x04000928 RID: 2344
		public int colonistsLaunched = 0;

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0007DD18 File Offset: 0x0007C118
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.numRaidsEnemy, "numRaidsEnemy", 0, false);
			Scribe_Values.Look<int>(ref this.numThreatBigs, "numThreatsQueued", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsKilled, "colonistsKilled", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsLaunched, "colonistsLaunched", 0, false);
		}
	}
}
