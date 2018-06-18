using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076E RID: 1902
	public class StockGenerator_BuyWeirdOrganic : StockGenerator
	{
		// Token: 0x06002A07 RID: 10759 RVA: 0x001639F0 File Offset: 0x00161DF0
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x00163A14 File Offset: 0x00161E14
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == ThingDefOf.InsectJelly;
		}
	}
}
