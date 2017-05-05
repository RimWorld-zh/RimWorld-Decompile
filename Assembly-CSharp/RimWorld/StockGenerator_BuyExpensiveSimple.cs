using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		public float minValuePerUnit = 15f;

		[DebuggerHidden]
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			StockGenerator_BuyExpensiveSimple.<GenerateThings>c__Iterator177 <GenerateThings>c__Iterator = new StockGenerator_BuyExpensiveSimple.<GenerateThings>c__Iterator177();
			StockGenerator_BuyExpensiveSimple.<GenerateThings>c__Iterator177 expr_07 = <GenerateThings>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && thingDef.BaseMarketValue / thingDef.VolumePerUnit > this.minValuePerUnit;
		}
	}
}
