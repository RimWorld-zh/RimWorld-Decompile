using System.Collections.Generic;
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

		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (!(tag == "MapScatteredWith"))
				yield break;
			yield return GenLabel.ThingLabel(base.thingDef, base.stuff, base.count).CapitalizeFirst();
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
