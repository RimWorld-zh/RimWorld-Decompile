using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076A RID: 1898
	public class StockGenerator_BuyWeirdOrganic : StockGenerator
	{
		// Token: 0x06002A00 RID: 10752 RVA: 0x00163BC8 File Offset: 0x00161FC8
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x00163BEC File Offset: 0x00161FEC
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == ThingDefOf.InsectJelly;
		}
	}
}
