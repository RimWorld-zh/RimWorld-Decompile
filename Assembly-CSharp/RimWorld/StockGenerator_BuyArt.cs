using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076D RID: 1901
	public class StockGenerator_BuyArt : StockGenerator
	{
		// Token: 0x06002A04 RID: 10756 RVA: 0x00163900 File Offset: 0x00161D00
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x00163924 File Offset: 0x00161D24
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.thingClass == typeof(Building_Art);
		}
	}
}
