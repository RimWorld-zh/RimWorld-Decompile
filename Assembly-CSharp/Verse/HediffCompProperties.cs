using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B1C RID: 2844
	public class HediffCompProperties
	{
		// Token: 0x0400284C RID: 10316
		[TranslationHandle]
		public Type compClass = null;

		// Token: 0x06003EC1 RID: 16065 RVA: 0x00211048 File Offset: 0x0020F448
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
