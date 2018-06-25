using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076C RID: 1900
	public class StockGenerator_BuyWeirdOrganic : StockGenerator
	{
		// Token: 0x06002A03 RID: 10755 RVA: 0x00163F78 File Offset: 0x00162378
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x00163F9C File Offset: 0x0016239C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == ThingDefOf.InsectJelly;
		}
	}
}
