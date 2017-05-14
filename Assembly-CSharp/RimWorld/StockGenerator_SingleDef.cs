using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class StockGenerator_SingleDef : StockGenerator
	{
		private ThingDef thingDef;

		[DebuggerHidden]
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			StockGenerator_SingleDef.<GenerateThings>c__Iterator17A <GenerateThings>c__Iterator17A = new StockGenerator_SingleDef.<GenerateThings>c__Iterator17A();
			<GenerateThings>c__Iterator17A.<>f__this = this;
			StockGenerator_SingleDef.<GenerateThings>c__Iterator17A expr_0E = <GenerateThings>c__Iterator17A;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}
	}
}
