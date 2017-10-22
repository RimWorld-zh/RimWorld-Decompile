using Verse;

namespace RimWorld
{
	public class StatsRecord : IExposable
	{
		public int numRaidsEnemy = 0;

		public int numThreatBigs = 0;

		public int colonistsKilled = 0;

		public int colonistsLaunched = 0;

		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.numRaidsEnemy, "numRaidsEnemy", 0, false);
			Scribe_Values.Look<int>(ref this.numThreatBigs, "numThreatsQueued", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsKilled, "colonistsKilled", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsLaunched, "colonistsLaunched", 0, false);
		}
	}
}
