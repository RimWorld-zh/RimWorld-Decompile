using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
	{
		public const string PlayerStartWithTag = "PlayerStartsWith";

		public static string PlayerStartWithIntro
		{
			get
			{
				return "ScenPart_StartWith".Translate();
			}
		}

		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (!(tag == "PlayerStartsWith"))
				yield break;
			yield return GenLabel.ThingLabel(base.thingDef, base.stuff, base.count).CapitalizeFirst();
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override IEnumerable<Thing> PlayerStartingThings()
		{
			Thing t = ThingMaker.MakeThing(base.thingDef, base.stuff);
			if (base.thingDef.Minifiable)
			{
				t = t.MakeMinified();
			}
			t.stackCount = base.count;
			yield return t;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
