using System;
using System.Collections.Generic;

namespace Verse
{
	public class HediffCompProperties
	{
		public Type compClass;

		public virtual IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "compClass is null";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			int i = 0;
			while (true)
			{
				if (i < parentDef.comps.Count)
				{
					if (parentDef.comps[i] != this && parentDef.comps[i].compClass == this.compClass)
						break;
					i++;
					continue;
				}
				yield break;
			}
			yield return "two comps with same compClass: " + this.compClass;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
