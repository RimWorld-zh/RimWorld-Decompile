using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076D RID: 1901
	public class StockGenerator_BuyArt : StockGenerator
	{
		// Token: 0x06002A02 RID: 10754 RVA: 0x0016386C File Offset: 0x00161C6C
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x00163890 File Offset: 0x00161C90
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.thingClass == typeof(Building_Art);
		}
	}
}
