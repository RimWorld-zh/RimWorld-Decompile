using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076F RID: 1903
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		// Token: 0x06002A08 RID: 10760 RVA: 0x00163A48 File Offset: 0x00161E48
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x00163A6C File Offset: 0x00161E6C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && thingDef.BaseMarketValue / thingDef.VolumePerUnit > this.minValuePerUnit;
		}

		// Token: 0x040016AF RID: 5807
		public float minValuePerUnit = 15f;
	}
}
