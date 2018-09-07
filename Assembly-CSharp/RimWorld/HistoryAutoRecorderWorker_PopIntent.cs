using System;

namespace RimWorld
{
	public class HistoryAutoRecorderWorker_PopIntent : HistoryAutoRecorderWorker
	{
		public HistoryAutoRecorderWorker_PopIntent()
		{
		}

		public override float PullRecord()
		{
			return StorytellerUtilityPopulation.PopulationIntent * 10f;
		}
	}
}
