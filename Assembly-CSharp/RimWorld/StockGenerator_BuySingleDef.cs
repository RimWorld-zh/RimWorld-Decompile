using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		public ThingDef thingDef;

		[DebuggerHidden]
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			StockGenerator_BuySingleDef.<GenerateThings>c__Iterator176 <GenerateThings>c__Iterator = new StockGenerator_BuySingleDef.<GenerateThings>c__Iterator176();
			StockGenerator_BuySingleDef.<GenerateThings>c__Iterator176 expr_07 = <GenerateThings>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}
	}
}
