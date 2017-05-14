using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_StartingThing_Defined.<GetSummaryListEntries>c__Iterator123 <GetSummaryListEntries>c__Iterator = new ScenPart_StartingThing_Defined.<GetSummaryListEntries>c__Iterator123();
			<GetSummaryListEntries>c__Iterator.tag = tag;
			<GetSummaryListEntries>c__Iterator.<$>tag = tag;
			<GetSummaryListEntries>c__Iterator.<>f__this = this;
			ScenPart_StartingThing_Defined.<GetSummaryListEntries>c__Iterator123 expr_1C = <GetSummaryListEntries>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		[DebuggerHidden]
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			ScenPart_StartingThing_Defined.<PlayerStartingThings>c__Iterator124 <PlayerStartingThings>c__Iterator = new ScenPart_StartingThing_Defined.<PlayerStartingThings>c__Iterator124();
			<PlayerStartingThings>c__Iterator.<>f__this = this;
			ScenPart_StartingThing_Defined.<PlayerStartingThings>c__Iterator124 expr_0E = <PlayerStartingThings>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
