using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076B RID: 1899
	public class StockGenerator_BuyArt : StockGenerator
	{
		// Token: 0x06002A01 RID: 10753 RVA: 0x00163C28 File Offset: 0x00162028
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x00163C4C File Offset: 0x0016204C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.thingClass == typeof(Building_Art);
		}
	}
}
