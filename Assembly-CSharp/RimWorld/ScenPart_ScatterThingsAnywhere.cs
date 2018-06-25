using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064D RID: 1613
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		// Token: 0x0400130F RID: 4879
		public const string MapScatteredWithTag = "MapScatteredWith";

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x0011C984 File Offset: 0x0011AD84
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x0011C99C File Offset: 0x0011AD9C
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x0011C9C8 File Offset: 0x0011ADC8
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "MapScatteredWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}
	}
}
