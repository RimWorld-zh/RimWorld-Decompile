using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000AF8 RID: 2808
	public class Editable
	{
		// Token: 0x06003E30 RID: 15920 RVA: 0x0006341E File Offset: 0x0006181E
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x00063421 File Offset: 0x00061821
		public virtual void PostLoad()
		{
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x00063424 File Offset: 0x00061824
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
