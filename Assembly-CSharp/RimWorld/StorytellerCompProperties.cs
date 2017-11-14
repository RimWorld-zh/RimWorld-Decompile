using System;
using System.Collections.Generic;

namespace RimWorld
{
	public class StorytellerCompProperties
	{
		public Type compClass;

		public float minDaysPassed;

		public List<IncidentTargetTypeDef> allowedTargetTypes;

		public float minIncChancePopulationIntentFactor = 0.05f;

		public StorytellerCompProperties()
		{
		}

		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.compClass != null)
				yield break;
			yield return parentDef.defName + " has StorytellerCompProperties with null compClass.";
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}
	}
}
