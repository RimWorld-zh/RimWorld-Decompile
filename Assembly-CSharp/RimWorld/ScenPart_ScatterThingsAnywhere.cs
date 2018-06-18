using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064F RID: 1615
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x0011C4CC File Offset: 0x0011A8CC
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x0011C4E4 File Offset: 0x0011A8E4
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x0011C510 File Offset: 0x0011A910
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "MapScatteredWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x0400130E RID: 4878
		public const string MapScatteredWithTag = "MapScatteredWith";
	}
}
