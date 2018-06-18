using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A2 RID: 2466
	public abstract class StatPart
	{
		// Token: 0x06003748 RID: 14152
		public abstract void TransformValue(StatRequest req, ref float val);

		// Token: 0x06003749 RID: 14153
		public abstract string ExplanationPart(StatRequest req);

		// Token: 0x0600374A RID: 14154 RVA: 0x001D8870 File Offset: 0x001D6C70
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
