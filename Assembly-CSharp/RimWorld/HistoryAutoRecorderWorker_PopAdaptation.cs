using System;
using Verse;

namespace RimWorld
{
	public class HistoryAutoRecorderWorker_PopAdaptation : HistoryAutoRecorderWorker
	{
		public HistoryAutoRecorderWorker_PopAdaptation()
		{
		}

		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherPopAdaptation.AdaptDays;
		}
	}
}
