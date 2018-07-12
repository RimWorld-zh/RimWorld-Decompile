using System;
using Verse;

namespace RimWorld
{
	public class HistoryAutoRecorderWorker_RampUp : HistoryAutoRecorderWorker
	{
		public HistoryAutoRecorderWorker_RampUp()
		{
		}

		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherRampUp.RampDays;
		}
	}
}
