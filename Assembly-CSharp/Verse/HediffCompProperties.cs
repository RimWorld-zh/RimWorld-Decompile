using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B1B RID: 2843
	public class HediffCompProperties
	{
		// Token: 0x04002845 RID: 10309
		[TranslationHandle]
		public Type compClass = null;

		// Token: 0x06003EC1 RID: 16065 RVA: 0x00210D68 File Offset: 0x0020F168
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
