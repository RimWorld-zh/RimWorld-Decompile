using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076D RID: 1901
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		// Token: 0x040016B1 RID: 5809
		public float minValuePerUnit = 15f;

		// Token: 0x06002A06 RID: 10758 RVA: 0x00164064 File Offset: 0x00162464
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x00164088 File Offset: 0x00162488
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && thingDef.BaseMarketValue / thingDef.VolumePerUnit > this.minValuePerUnit;
		}
	}
}
