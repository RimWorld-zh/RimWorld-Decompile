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
			ScenPart_ScatterThingsAnywhere.<GetSummaryListEntries>c__Iterator11F <GetSummaryListEntries>c__Iterator11F = new ScenPart_ScatterThingsAnywhere.<GetSummaryListEntries>c__Iterator11F();
			<GetSummaryListEntries>c__Iterator11F.tag = tag;
			<GetSummaryListEntries>c__Iterator11F.<$>tag = tag;
			<GetSummaryListEntries>c__Iterator11F.<>f__this = this;
			ScenPart_ScatterThingsAnywhere.<GetSummaryListEntries>c__Iterator11F expr_1C = <GetSummaryListEntries>c__Iterator11F;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
