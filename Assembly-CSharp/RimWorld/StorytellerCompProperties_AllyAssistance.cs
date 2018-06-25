using System;

namespace RimWorld
{
	public class StorytellerCompProperties_AllyAssistance : StorytellerCompProperties
	{
		public float baseMtbDays = 99999f;

		public StorytellerCompProperties_AllyAssistance()
		{
			this.compClass = typeof(StorytellerComp_AllyAssistance);
		}
	}
}
