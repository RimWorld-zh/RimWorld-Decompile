using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064E RID: 1614
	public class ScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x0011C8E4 File Offset: 0x0011ACE4
		protected override bool NearPlayerStart
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x0011C8FC File Offset: 0x0011ACFC
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x0011C924 File Offset: 0x0011AD24
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}
	}
}
