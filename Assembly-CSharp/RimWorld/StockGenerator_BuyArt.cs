using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076B RID: 1899
	public class StockGenerator_BuyArt : StockGenerator
	{
		// Token: 0x06002A00 RID: 10752 RVA: 0x00163E88 File Offset: 0x00162288
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x00163EAC File Offset: 0x001622AC
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.thingClass == typeof(Building_Art);
		}
	}
}
