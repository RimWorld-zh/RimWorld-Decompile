using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class StockGenerator_Slaves : StockGenerator
	{
		[DebuggerHidden]
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			StockGenerator_Slaves.<GenerateThings>c__Iterator17E <GenerateThings>c__Iterator17E = new StockGenerator_Slaves.<GenerateThings>c__Iterator17E();
			<GenerateThings>c__Iterator17E.forTile = forTile;
			<GenerateThings>c__Iterator17E.<$>forTile = forTile;
			<GenerateThings>c__Iterator17E.<>f__this = this;
			StockGenerator_Slaves.<GenerateThings>c__Iterator17E expr_1C = <GenerateThings>c__Iterator17E;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike;
		}
	}
}
