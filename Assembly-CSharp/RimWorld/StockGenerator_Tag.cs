using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class StockGenerator_Tag : StockGenerator
	{
		private string tradeTag;

		private IntRange thingDefCountRange = IntRange.one;

		[DebuggerHidden]
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			StockGenerator_Tag.<GenerateThings>c__Iterator17D <GenerateThings>c__Iterator17D = new StockGenerator_Tag.<GenerateThings>c__Iterator17D();
			<GenerateThings>c__Iterator17D.<>f__this = this;
			StockGenerator_Tag.<GenerateThings>c__Iterator17D expr_0E = <GenerateThings>c__Iterator17D;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability == Tradeability.Stockable && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}
	}
}
