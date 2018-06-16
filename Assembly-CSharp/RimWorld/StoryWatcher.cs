using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200035B RID: 859
	public sealed class StoryWatcher : IExposable
	{
		// Token: 0x06000EE9 RID: 3817 RVA: 0x0007DBA0 File Offset: 0x0007BFA0
		public void StoryWatcherTick()
		{
			this.watcherRampUp.RampUpWatcherTick();
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0007DBAE File Offset: 0x0007BFAE
		public void ExposeData()
		{
			Scribe_Deep.Look<StatsRecord>(ref this.statsRecord, "statsRecord", new object[0]);
			Scribe_Deep.Look<StoryWatcher_RampUp>(ref this.watcherRampUp, "watcherRampUp", new object[0]);
		}

		// Token: 0x04000927 RID: 2343
		public StatsRecord statsRecord = new StatsRecord();

		// Token: 0x04000928 RID: 2344
		public StoryWatcher_RampUp watcherRampUp = new StoryWatcher_RampUp();
	}
}
