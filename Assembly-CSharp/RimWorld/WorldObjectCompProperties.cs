using RimWorld.Planet;
using System;
using System.Collections.Generic;

namespace RimWorld
{
	public class WorldObjectCompProperties
	{
		public Type compClass = typeof(WorldObjectComp);

		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			if (this.compClass != null)
				yield break;
			yield return parentDef.defName + " has WorldObjectCompProperties with null compClass.";
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}
	}
}
