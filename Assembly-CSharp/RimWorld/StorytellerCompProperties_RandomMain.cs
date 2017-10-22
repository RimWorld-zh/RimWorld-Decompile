using System.Collections.Generic;

namespace RimWorld
{
	public class StorytellerCompProperties_RandomMain : StorytellerCompProperties
	{
		public float mtbDays;

		public List<IncidentCategoryEntry> categoryWeights = new List<IncidentCategoryEntry>();

		public float maxThreatBigIntervalDays = 99999f;

		public StorytellerCompProperties_RandomMain()
		{
			base.compClass = typeof(StorytellerComp_RandomMain);
		}
	}
}
