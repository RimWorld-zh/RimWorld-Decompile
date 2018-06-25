using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076E RID: 1902
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		// Token: 0x040016B2 RID: 5810
		public ThingDef thingDef = null;

		// Token: 0x06002A09 RID: 10761 RVA: 0x00164194 File Offset: 0x00162594
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x001641B8 File Offset: 0x001625B8
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}
	}
}
