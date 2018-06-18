using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000777 RID: 1911
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		// Token: 0x06002A2B RID: 10795 RVA: 0x00165A34 File Offset: 0x00163E34
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

		// Token: 0x06002A2C RID: 10796 RVA: 0x00165A60 File Offset: 0x00163E60
		protected virtual Thing MakeThing(ThingDef def)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1);
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x00165A7C File Offset: 0x00163E7C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy;
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x00165AB0 File Offset: 0x00163EB0
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
