using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000272 RID: 626
	public class SkillNeed
	{
		// Token: 0x06000ACA RID: 2762 RVA: 0x00061CD8 File Offset: 0x000600D8
		public virtual float ValueFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00061CE0 File Offset: 0x000600E0
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x04000554 RID: 1364
		public SkillDef skill;
	}
}
