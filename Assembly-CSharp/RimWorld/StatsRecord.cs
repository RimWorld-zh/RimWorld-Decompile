using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200035C RID: 860
	public class StatsRecord : IExposable
	{
		// Token: 0x04000928 RID: 2344
		public int numRaidsEnemy = 0;

		// Token: 0x04000929 RID: 2345
		public int numThreatBigs = 0;

		// Token: 0x0400092A RID: 2346
		public int colonistsKilled = 0;

		// Token: 0x0400092B RID: 2347
		public int colonistsLaunched = 0;

		// Token: 0x06000EEA RID: 3818 RVA: 0x0007DE78 File Offset: 0x0007C278
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.numRaidsEnemy, "numRaidsEnemy", 0, false);
			Scribe_Values.Look<int>(ref this.numThreatBigs, "numThreatsQueued", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsKilled, "colonistsKilled", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsLaunched, "colonistsLaunched", 0, false);
		}
	}
}
