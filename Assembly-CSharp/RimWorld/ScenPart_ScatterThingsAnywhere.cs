using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064F RID: 1615
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x0011C454 File Offset: 0x0011A854
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x0011C46C File Offset: 0x0011A86C
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x0011C498 File Offset: 0x0011A898
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
