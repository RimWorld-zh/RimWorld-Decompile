using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A2 RID: 2466
	public abstract class StatPart
	{
		// Token: 0x06003746 RID: 14150
		public abstract void TransformValue(StatRequest req, ref float val);

		// Token: 0x06003747 RID: 14151
		public abstract string ExplanationPart(StatRequest req);

		// Token: 0x06003748 RID: 14152 RVA: 0x001D879C File Offset: 0x001D6B9C
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x04002393 RID: 9107
		public float priority;

		// Token: 0x04002394 RID: 9108
		[Unsaved]
		public StatDef parentStat;
	}
}
