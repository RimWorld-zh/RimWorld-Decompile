using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076E RID: 1902
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		// Token: 0x040016AE RID: 5806
		public ThingDef thingDef = null;

		// Token: 0x06002A0A RID: 10762 RVA: 0x00163F34 File Offset: 0x00162334
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x00163F58 File Offset: 0x00162358
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}
	}
}
