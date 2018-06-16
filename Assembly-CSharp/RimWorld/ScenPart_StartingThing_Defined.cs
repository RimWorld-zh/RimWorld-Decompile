using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000651 RID: 1617
	public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
	{
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x0011C7E0 File Offset: 0x0011ABE0
		public static string PlayerStartWithIntro
		{
			get
			{
				return "ScenPart_StartWith".Translate();
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x0011C800 File Offset: 0x0011AC00
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x0011C828 File Offset: 0x0011AC28
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x0011C85C File Offset: 0x0011AC5C
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

		// Token: 0x0400130F RID: 4879
		public const string PlayerStartWithTag = "PlayerStartsWith";
	}
}
