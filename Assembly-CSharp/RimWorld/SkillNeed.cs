using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000274 RID: 628
	public class SkillNeed
	{
		// Token: 0x04000554 RID: 1364
		public SkillDef skill;

		// Token: 0x06000ACB RID: 2763 RVA: 0x00061E80 File Offset: 0x00060280
		public virtual float ValueFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00061E88 File Offset: 0x00060288
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
