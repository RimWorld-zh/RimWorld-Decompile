using System;

namespace RimWorld
{
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		public float baseMtbDays = 99999f;

		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}
	}
}
