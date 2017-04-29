using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		[DebuggerHidden]
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			StockGenerator_MiscItems.<GenerateThings>c__Iterator17E <GenerateThings>c__Iterator17E = new StockGenerator_MiscItems.<GenerateThings>c__Iterator17E();
			<GenerateThings>c__Iterator17E.<>f__this = this;
			StockGenerator_MiscItems.<GenerateThings>c__Iterator17E expr_0E = <GenerateThings>c__Iterator17E;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		protected virtual Thing MakeThing(ThingDef def)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1);
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability == Tradeability.Stockable && thingDef.techLevel <= this.maxTechLevelBuy;
		}

		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
