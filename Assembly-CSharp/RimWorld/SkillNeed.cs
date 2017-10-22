using System;
using System.Collections.Generic;
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

		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
