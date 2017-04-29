using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class SkillNeed
	{
		public SkillDef skill;

		public bool reportInverse;

		public virtual float FactorFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		[DebuggerHidden]
		public virtual IEnumerable<string> ConfigErrors()
		{
			SkillNeed.<ConfigErrors>c__Iterator85 <ConfigErrors>c__Iterator = new SkillNeed.<ConfigErrors>c__Iterator85();
			SkillNeed.<ConfigErrors>c__Iterator85 expr_07 = <ConfigErrors>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}
	}
}
