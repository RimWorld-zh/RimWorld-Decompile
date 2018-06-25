using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076C RID: 1900
	public class StockGenerator_BuyWeirdOrganic : StockGenerator
	{
		// Token: 0x06002A04 RID: 10756 RVA: 0x00163D18 File Offset: 0x00162118
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x00163D3C File Offset: 0x0016213C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == ThingDefOf.InsectJelly;
		}
	}
}
