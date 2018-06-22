using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000272 RID: 626
	public class SkillNeed
	{
		// Token: 0x06000AC8 RID: 2760 RVA: 0x00061D34 File Offset: 0x00060134
		public virtual float ValueFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00061D3C File Offset: 0x0006013C
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x04000552 RID: 1362
		public SkillDef skill;
	}
}
