using System;

namespace RimWorld
{
	public class StorytellerCompProperties_Disease : StorytellerCompProperties
	{
		public IncidentCategoryDef incidentCategory;

		public StorytellerCompProperties_Disease()
		{
			this.compClass = typeof(StorytellerComp_Disease);
		}
	}
}
