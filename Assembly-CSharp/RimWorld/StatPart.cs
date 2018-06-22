using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099E RID: 2462
	public abstract class StatPart
	{
		// Token: 0x06003741 RID: 14145
		public abstract void TransformValue(StatRequest req, ref float val);

		// Token: 0x06003742 RID: 14146
		public abstract string ExplanationPart(StatRequest req);

		// Token: 0x06003743 RID: 14147 RVA: 0x001D8A6C File Offset: 0x001D6E6C
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x04002391 RID: 9105
		public float priority;

		// Token: 0x04002392 RID: 9106
		[Unsaved]
		public StatDef parentStat;
	}
}
