using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B1D RID: 2845
	public class HediffCompProperties
	{
		// Token: 0x06003EBF RID: 16063 RVA: 0x0021082C File Offset: 0x0020EC2C
		public virtual IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "compClass is null";
			}
			for (int i = 0; i < parentDef.comps.Count; i++)
			{
				if (parentDef.comps[i] != this && parentDef.comps[i].compClass == this.compClass)
				{
					yield return "two comps with same compClass: " + this.compClass;
				}
			}
			yield break;
		}

		// Token: 0x04002848 RID: 10312
		public Type compClass = null;
	}
}
