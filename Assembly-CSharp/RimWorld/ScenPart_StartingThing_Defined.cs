using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064F RID: 1615
	public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
	{
		// Token: 0x0400130C RID: 4876
		public const string PlayerStartWithTag = "PlayerStartsWith";

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x0011CAA8 File Offset: 0x0011AEA8
		public static string PlayerStartWithIntro
		{
			get
			{
				return "ScenPart_StartWith".Translate();
			}
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0011CAC8 File Offset: 0x0011AEC8
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x0011CAF0 File Offset: 0x0011AEF0
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x0011CB24 File Offset: 0x0011AF24
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			Thing t = ThingMaker.MakeThing(this.thingDef, this.stuff);
			if (this.thingDef.Minifiable)
			{
				t = t.MakeMinified();
			}
			t.stackCount = this.count;
			yield return t;
			yield break;
		}
	}
}
