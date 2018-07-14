using System;
using Verse;

namespace RimWorld
{
	public class HistoryAutoRecorderWorker_ThreatPoints : HistoryAutoRecorderWorker
	{
		public HistoryAutoRecorderWorker_ThreatPoints()
		{
		}

		public override float PullRecord()
		{
			return StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap) / 10f;
		}
	}
}
