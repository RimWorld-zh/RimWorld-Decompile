using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000775 RID: 1909
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		// Token: 0x06002A27 RID: 10791 RVA: 0x00165FBC File Offset: 0x001643BC
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			int count = this.countRange.RandomInRange;
			for (int i = 0; i < count; i++)
			{
				ThingDef finalDef;
				if (!(from t in DefDatabase<ThingDef>.AllDefs
				where this.HandlesThingDef(t) && t.tradeability.TraderCanSell() && t.techLevel <= this.maxTechLevelGenerate
				select t).TryRandomElementByWeight(new Func<ThingDef, float>(this.SelectionWeight), out finalDef))
				{
					yield break;
				}
				yield return this.MakeThing(finalDef);
			}
			yield break;
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x00165FE8 File Offset: 0x001643E8
		protected virtual Thing MakeThing(ThingDef def)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1);
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x00166004 File Offset: 0x00164404
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy;
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x00166038 File Offset: 0x00164438
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
