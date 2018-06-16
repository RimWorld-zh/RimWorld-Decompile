using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076E RID: 1902
	public class StockGenerator_BuyWeirdOrganic : StockGenerator
	{
		// Token: 0x06002A05 RID: 10757 RVA: 0x0016395C File Offset: 0x00161D5C
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x00163980 File Offset: 0x00161D80
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == ThingDefOf.InsectJelly;
		}
	}
}
