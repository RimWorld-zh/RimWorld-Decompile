using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000AF7 RID: 2807
	public class Editable
	{
		// Token: 0x06003E30 RID: 15920 RVA: 0x00063422 File Offset: 0x00061822
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x00063425 File Offset: 0x00061825
		public virtual void PostLoad()
		{
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x00063428 File Offset: 0x00061828
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
