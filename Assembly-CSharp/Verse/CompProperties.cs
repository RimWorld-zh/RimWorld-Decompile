using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse
{
	public class CompProperties
	{
		public Type compClass = typeof(ThingComp);

		public CompProperties()
		{
		}

		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass != null)
				yield break;
			yield return parentDef.defName + " has CompProperties with null compClass.";
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield break;
		}
	}
}
