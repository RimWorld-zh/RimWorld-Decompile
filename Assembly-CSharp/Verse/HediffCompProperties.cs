using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B19 RID: 2841
	public class HediffCompProperties
	{
		// Token: 0x04002844 RID: 10308
		[TranslationHandle]
		public Type compClass = null;

		// Token: 0x06003EBD RID: 16061 RVA: 0x00210C3C File Offset: 0x0020F03C
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
	}
}
