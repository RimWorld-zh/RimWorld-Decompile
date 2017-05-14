using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public abstract class TaleData : IExposable
	{
		public abstract void ExposeData();

		[DebuggerHidden]
		public virtual IEnumerable<Rule> GetRules(string prefix)
		{
			TaleData.<GetRules>c__Iterator12A <GetRules>c__Iterator12A = new TaleData.<GetRules>c__Iterator12A();
			<GetRules>c__Iterator12A.<>f__this = this;
			TaleData.<GetRules>c__Iterator12A expr_0E = <GetRules>c__Iterator12A;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		[DebuggerHidden]
		public virtual IEnumerable<Rule> GetRules()
		{
			TaleData.<GetRules>c__Iterator12B <GetRules>c__Iterator12B = new TaleData.<GetRules>c__Iterator12B();
			<GetRules>c__Iterator12B.<>f__this = this;
			TaleData.<GetRules>c__Iterator12B expr_0E = <GetRules>c__Iterator12B;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
