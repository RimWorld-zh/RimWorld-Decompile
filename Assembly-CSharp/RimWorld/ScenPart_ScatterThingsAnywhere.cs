using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		public const string MapScatteredWithTag = "MapScatteredWith";

		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		[DebuggerHidden]
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_ScatterThingsAnywhere.<GetSummaryListEntries>c__Iterator121 <GetSummaryListEntries>c__Iterator = new ScenPart_ScatterThingsAnywhere.<GetSummaryListEntries>c__Iterator121();
			<GetSummaryListEntries>c__Iterator.tag = tag;
			<GetSummaryListEntries>c__Iterator.<$>tag = tag;
			<GetSummaryListEntries>c__Iterator.<>f__this = this;
			ScenPart_ScatterThingsAnywhere.<GetSummaryListEntries>c__Iterator121 expr_1C = <GetSummaryListEntries>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
