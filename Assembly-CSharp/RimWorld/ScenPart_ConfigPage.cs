using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class ScenPart_ConfigPage : ScenPart
	{
		[DebuggerHidden]
		public override IEnumerable<Page> GetConfigPages()
		{
			ScenPart_ConfigPage.<GetConfigPages>c__Iterator120 <GetConfigPages>c__Iterator = new ScenPart_ConfigPage.<GetConfigPages>c__Iterator120();
			<GetConfigPages>c__Iterator.<>f__this = this;
			ScenPart_ConfigPage.<GetConfigPages>c__Iterator120 expr_0E = <GetConfigPages>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}
	}
}
