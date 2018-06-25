using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064D RID: 1613
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		// Token: 0x0400130B RID: 4875
		public const string MapScatteredWithTag = "MapScatteredWith";

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x0011C71C File Offset: 0x0011AB1C
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x0011C734 File Offset: 0x0011AB34
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x0011C760 File Offset: 0x0011AB60
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
