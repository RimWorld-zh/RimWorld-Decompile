using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000274 RID: 628
	public class SkillNeed
	{
		// Token: 0x04000552 RID: 1362
		public SkillDef skill;

		// Token: 0x06000ACC RID: 2764 RVA: 0x00061E84 File Offset: 0x00060284
		public virtual float ValueFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00061E8C File Offset: 0x0006028C
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
