using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class StockGenerator_MultiDef : StockGenerator
	{
		private List<ThingDef> thingDefs = new List<ThingDef>();

		[DebuggerHidden]
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			StockGenerator_MultiDef.<GenerateThings>c__Iterator178 <GenerateThings>c__Iterator = new StockGenerator_MultiDef.<GenerateThings>c__Iterator178();
			<GenerateThings>c__Iterator.<>f__this = this;
			StockGenerator_MultiDef.<GenerateThings>c__Iterator178 expr_0E = <GenerateThings>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}
	}
}
