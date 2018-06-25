using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A0 RID: 2464
	public abstract class StatPart
	{
		// Token: 0x04002392 RID: 9106
		public float priority;

		// Token: 0x04002393 RID: 9107
		[Unsaved]
		public StatDef parentStat;

		// Token: 0x06003745 RID: 14149
		public abstract void TransformValue(StatRequest req, ref float val);

		// Token: 0x06003746 RID: 14150
		public abstract string ExplanationPart(StatRequest req);

		// Token: 0x06003747 RID: 14151 RVA: 0x001D8BAC File Offset: 0x001D6FAC
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
