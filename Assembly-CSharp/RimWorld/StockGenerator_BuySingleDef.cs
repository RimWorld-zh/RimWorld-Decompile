using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076C RID: 1900
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		// Token: 0x06002A06 RID: 10758 RVA: 0x00163DE4 File Offset: 0x001621E4
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x00163E08 File Offset: 0x00162208
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x040016AE RID: 5806
		public ThingDef thingDef = null;
	}
}
