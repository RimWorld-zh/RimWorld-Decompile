using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064D RID: 1613
	public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
	{
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x0011C958 File Offset: 0x0011AD58
		public static string PlayerStartWithIntro
		{
			get
			{
				return "ScenPart_StartWith".Translate();
			}
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x0011C978 File Offset: 0x0011AD78
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x0011C9A0 File Offset: 0x0011ADA0
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x0011C9D4 File Offset: 0x0011ADD4
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

		// Token: 0x0400130C RID: 4876
		public const string PlayerStartWithTag = "PlayerStartsWith";
	}
}
