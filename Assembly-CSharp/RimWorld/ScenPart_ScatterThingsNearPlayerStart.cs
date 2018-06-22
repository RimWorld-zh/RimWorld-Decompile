using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064C RID: 1612
	public class ScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x0011C794 File Offset: 0x0011AB94
		protected override bool NearPlayerStart
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x0011C7AC File Offset: 0x0011ABAC
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x0011C7D4 File Offset: 0x0011ABD4
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
