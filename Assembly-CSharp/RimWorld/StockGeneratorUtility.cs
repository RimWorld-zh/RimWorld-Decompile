using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StockGeneratorUtility
	{
		public static IEnumerable<Thing> TryMakeForStock(ThingDef thingDef, int count)
		{
			if (thingDef.MadeFromStuff)
			{
				int i = 0;
				Thing th2;
				while (true)
				{
					if (i < count)
					{
						th2 = StockGeneratorUtility.TryMakeForStockSingle(thingDef, 1);
						if (th2 == null)
						{
							i++;
							continue;
						}
						break;
					}
					yield break;
				}
				yield return th2;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			Thing th = StockGeneratorUtility.TryMakeForStockSingle(thingDef, count);
			if (th == null)
				yield break;
			yield return th;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public static Thing TryMakeForStockSingle(ThingDef thingDef, int stackCount)
		{
			Thing result;
			if (stackCount <= 0)
			{
				result = null;
			}
			else if (thingDef.tradeability != Tradeability.Stockable)
			{
				Log.Error("Tried to make non-Stockable thing for trader stock: " + thingDef);
				result = null;
			}
			else
			{
				ThingDef stuff = null;
				if (thingDef.MadeFromStuff)
				{
					stuff = GenStuff.RandomStuffByCommonalityFor(thingDef, TechLevel.Undefined);
				}
				Thing thing = ThingMaker.MakeThing(thingDef, stuff);
				thing.stackCount = stackCount;
				result = thing;
			}
			return result;
		}
	}
}
