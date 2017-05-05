using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class StockGeneratorUtility
	{
		[DebuggerHidden]
		public static IEnumerable<Thing> TryMakeForStock(ThingDef thingDef, int count)
		{
			StockGeneratorUtility.<TryMakeForStock>c__Iterator174 <TryMakeForStock>c__Iterator = new StockGeneratorUtility.<TryMakeForStock>c__Iterator174();
			<TryMakeForStock>c__Iterator.thingDef = thingDef;
			<TryMakeForStock>c__Iterator.count = count;
			<TryMakeForStock>c__Iterator.<$>thingDef = thingDef;
			<TryMakeForStock>c__Iterator.<$>count = count;
			StockGeneratorUtility.<TryMakeForStock>c__Iterator174 expr_23 = <TryMakeForStock>c__Iterator;
			expr_23.$PC = -2;
			return expr_23;
		}

		public static Thing TryMakeForStockSingle(ThingDef thingDef, int stackCount)
		{
			if (stackCount <= 0)
			{
				return null;
			}
			if (thingDef.tradeability != Tradeability.Stockable)
			{
				Log.Error("Tried to make non-Stockable thing for trader stock: " + thingDef);
				return null;
			}
			ThingDef stuff = null;
			if (thingDef.MadeFromStuff)
			{
				stuff = (from st in DefDatabase<ThingDef>.AllDefs
				where st.IsStuff && st.stuffProps.CanMake(thingDef)
				select st).RandomElementByWeight((ThingDef st) => st.stuffProps.commonality);
			}
			Thing thing = ThingMaker.MakeThing(thingDef, stuff);
			thing.stackCount = stackCount;
			return thing;
		}
	}
}
