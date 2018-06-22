using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000768 RID: 1896
	public class StockGeneratorUtility
	{
		// Token: 0x060029F9 RID: 10745 RVA: 0x001637B4 File Offset: 0x00161BB4
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

		// Token: 0x060029FA RID: 10746 RVA: 0x001637E8 File Offset: 0x00161BE8
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
