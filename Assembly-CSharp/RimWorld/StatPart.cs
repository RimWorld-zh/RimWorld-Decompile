using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public abstract class StatPart
	{
		[Unsaved]
		public StatDef parentStat;

		public abstract void TransformValue(StatRequest req, ref float val);

		public abstract string ExplanationPart(StatRequest req);

		[DebuggerHidden]
		public virtual IEnumerable<string> ConfigErrors()
		{
			StatPart.<ConfigErrors>c__Iterator1AA <ConfigErrors>c__Iterator1AA = new StatPart.<ConfigErrors>c__Iterator1AA();
			StatPart.<ConfigErrors>c__Iterator1AA expr_07 = <ConfigErrors>c__Iterator1AA;
			expr_07.$PC = -2;
			return expr_07;
		}
	}
}
