using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200035D RID: 861
	public sealed class StoryWatcher : IExposable
	{
		// Token: 0x0400092C RID: 2348
		public StatsRecord statsRecord = new StatsRecord();

		// Token: 0x0400092D RID: 2349
		public StoryWatcher_RampUp watcherRampUp = new StoryWatcher_RampUp();

		// Token: 0x06000EEC RID: 3820 RVA: 0x0007DEEC File Offset: 0x0007C2EC
		public void StoryWatcherTick()
		{
			this.watcherRampUp.RampUpWatcherTick();
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0007DEFA File Offset: 0x0007C2FA
		public void ExposeData()
		{
			Scribe_Deep.Look<StatsRecord>(ref this.statsRecord, "statsRecord", new object[0]);
			Scribe_Deep.Look<StoryWatcher_RampUp>(ref this.watcherRampUp, "watcherRampUp", new object[0]);
		}
	}
}
