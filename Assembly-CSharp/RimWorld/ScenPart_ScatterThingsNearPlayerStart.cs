using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000650 RID: 1616
	public class ScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x0011C694 File Offset: 0x0011AA94
		protected override bool NearPlayerStart
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0011C6AC File Offset: 0x0011AAAC
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x0011C6D4 File Offset: 0x0011AAD4
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
