using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000650 RID: 1616
	public class ScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x0011C61C File Offset: 0x0011AA1C
		protected override bool NearPlayerStart
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x0011C634 File Offset: 0x0011AA34
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x0011C65C File Offset: 0x0011AA5C
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
