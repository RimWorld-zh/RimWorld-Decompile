using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class StockGenerator_BuyWeirdOrganic : StockGenerator
	{
		[DebuggerHidden]
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			StockGenerator_BuyWeirdOrganic.<GenerateThings>c__Iterator175 <GenerateThings>c__Iterator = new StockGenerator_BuyWeirdOrganic.<GenerateThings>c__Iterator175();
			StockGenerator_BuyWeirdOrganic.<GenerateThings>c__Iterator175 expr_07 = <GenerateThings>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == ThingDefOf.InsectJelly;
		}
	}
}
