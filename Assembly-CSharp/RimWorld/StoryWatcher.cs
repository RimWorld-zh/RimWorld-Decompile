using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200035B RID: 859
	public sealed class StoryWatcher : IExposable
	{
		// Token: 0x04000929 RID: 2345
		public StatsRecord statsRecord = new StatsRecord();

		// Token: 0x0400092A RID: 2346
		public StoryWatcher_RampUp watcherRampUp = new StoryWatcher_RampUp();

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0007DD8C File Offset: 0x0007C18C
		public void StoryWatcherTick()
		{
			this.watcherRampUp.RampUpWatcherTick();
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0007DD9A File Offset: 0x0007C19A
		public void ExposeData()
		{
			Scribe_Deep.Look<StatsRecord>(ref this.statsRecord, "statsRecord", new object[0]);
			Scribe_Deep.Look<StoryWatcher_RampUp>(ref this.watcherRampUp, "watcherRampUp", new object[0]);
		}
	}
}
