using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000777 RID: 1911
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		// Token: 0x06002A29 RID: 10793 RVA: 0x001659A0 File Offset: 0x00163DA0
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

		// Token: 0x06002A2A RID: 10794 RVA: 0x001659CC File Offset: 0x00163DCC
		protected virtual Thing MakeThing(ThingDef def)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1);
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x001659E8 File Offset: 0x00163DE8
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy;
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x00165A1C File Offset: 0x00163E1C
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
