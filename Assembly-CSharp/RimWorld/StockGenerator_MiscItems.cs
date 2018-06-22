using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000773 RID: 1907
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		// Token: 0x06002A24 RID: 10788 RVA: 0x00165C0C File Offset: 0x0016400C
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

		// Token: 0x06002A25 RID: 10789 RVA: 0x00165C38 File Offset: 0x00164038
		protected virtual Thing MakeThing(ThingDef def)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1);
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x00165C54 File Offset: 0x00164054
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy;
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x00165C88 File Offset: 0x00164088
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
