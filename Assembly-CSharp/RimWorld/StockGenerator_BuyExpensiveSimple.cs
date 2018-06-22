using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076B RID: 1899
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		// Token: 0x06002A03 RID: 10755 RVA: 0x00163CB4 File Offset: 0x001620B4
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x00163CD8 File Offset: 0x001620D8
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && thingDef.BaseMarketValue / thingDef.VolumePerUnit > this.minValuePerUnit;
		}

		// Token: 0x040016AD RID: 5805
		public float minValuePerUnit = 15f;
	}
}
