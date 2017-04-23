using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	public class HediffCompProperties
	{
		public Type compClass;

		[DebuggerHidden]
		public IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			HediffCompProperties.<ConfigErrors>c__Iterator1C1 <ConfigErrors>c__Iterator1C = new HediffCompProperties.<ConfigErrors>c__Iterator1C1();
			<ConfigErrors>c__Iterator1C.parentDef = parentDef;
			<ConfigErrors>c__Iterator1C.<$>parentDef = parentDef;
			<ConfigErrors>c__Iterator1C.<>f__this = this;
			HediffCompProperties.<ConfigErrors>c__Iterator1C1 expr_1C = <ConfigErrors>c__Iterator1C;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
