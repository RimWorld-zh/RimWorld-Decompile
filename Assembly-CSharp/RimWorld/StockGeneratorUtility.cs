using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076C RID: 1900
	public class StockGeneratorUtility
	{
		// Token: 0x060029FE RID: 10750 RVA: 0x00163548 File Offset: 0x00161948
		public static IEnumerable<Thing> TryMakeForStock(ThingDef thingDef, int count)
		{
			if (thingDef.MadeFromStuff)
			{
				for (int i = 0; i < count; i++)
				{
					Thing th = StockGeneratorUtility.TryMakeForStockSingle(thingDef, 1);
					if (th != null)
					{
						yield return th;
					}
				}
			}
			else
			{
				Thing th2 = StockGeneratorUtility.TryMakeForStockSingle(thingDef, count);
				if (th2 != null)
				{
					yield return th2;
				}
			}
			yield break;
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x0016357C File Offset: 0x0016197C
		public static Thing TryMakeForStockSingle(ThingDef thingDef, int stackCount)
		{
			Thing result;
			if (stackCount <= 0)
			{
				result = null;
			}
			else if (!thingDef.tradeability.TraderCanSell())
			{
				Log.Error("Tried to make non-trader-sellable thing for trader stock: " + thingDef, false);
				result = null;
			}
			else
			{
				ThingDef stuff = null;
				if (thingDef.MadeFromStuff)
				{
					if (!(from x in GenStuff.AllowedStuffsFor(thingDef, TechLevel.Undefined)
					where !PawnWeaponGenerator.IsDerpWeapon(thingDef, x)
					select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff))
					{
						stuff = GenStuff.RandomStuffByCommonalityFor(thingDef, TechLevel.Undefined);
					}
				}
				Thing thing = ThingMaker.MakeThing(thingDef, stuff);
				thing.stackCount = stackCount;
				result = thing;
			}
			return result;
		}
	}
}
