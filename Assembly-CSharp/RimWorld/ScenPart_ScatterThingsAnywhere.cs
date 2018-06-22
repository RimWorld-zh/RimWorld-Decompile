using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064B RID: 1611
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x0011C5CC File Offset: 0x0011A9CC
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x0011C5E4 File Offset: 0x0011A9E4
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x0011C610 File Offset: 0x0011AA10
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "MapScatteredWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x0400130B RID: 4875
		public const string MapScatteredWithTag = "MapScatteredWith";
	}
}
