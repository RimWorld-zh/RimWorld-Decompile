using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000AF5 RID: 2805
	public class Editable
	{
		// Token: 0x06003E2C RID: 15916 RVA: 0x000632D2 File Offset: 0x000616D2
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x000632D5 File Offset: 0x000616D5
		public virtual void PostLoad()
		{
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x000632D8 File Offset: 0x000616D8
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
