using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorld
{
	public class ScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
	{
		protected override bool NearPlayerStart
		{
			get
			{
				return true;
			}
		}

		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		[DebuggerHidden]
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_ScatterThingsNearPlayerStart.<GetSummaryListEntries>c__Iterator121 <GetSummaryListEntries>c__Iterator = new ScenPart_ScatterThingsNearPlayerStart.<GetSummaryListEntries>c__Iterator121();
			<GetSummaryListEntries>c__Iterator.tag = tag;
			<GetSummaryListEntries>c__Iterator.<$>tag = tag;
			<GetSummaryListEntries>c__Iterator.<>f__this = this;
			ScenPart_ScatterThingsNearPlayerStart.<GetSummaryListEntries>c__Iterator121 expr_1C = <GetSummaryListEntries>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
