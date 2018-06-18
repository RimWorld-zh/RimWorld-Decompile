using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000AF9 RID: 2809
	public class Editable
	{
		// Token: 0x06003E31 RID: 15921 RVA: 0x00063276 File Offset: 0x00061676
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x00063279 File Offset: 0x00061679
		public virtual void PostLoad()
		{
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x0006327C File Offset: 0x0006167C
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
