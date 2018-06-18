using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076F RID: 1903
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		// Token: 0x06002A0A RID: 10762 RVA: 0x00163ADC File Offset: 0x00161EDC
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x00163B00 File Offset: 0x00161F00
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && thingDef.BaseMarketValue / thingDef.VolumePerUnit > this.minValuePerUnit;
		}

		// Token: 0x040016AF RID: 5807
		public float minValuePerUnit = 15f;
	}
}
