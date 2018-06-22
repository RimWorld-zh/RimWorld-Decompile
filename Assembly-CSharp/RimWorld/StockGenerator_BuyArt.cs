using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000769 RID: 1897
	public class StockGenerator_BuyArt : StockGenerator
	{
		// Token: 0x060029FD RID: 10749 RVA: 0x00163AD8 File Offset: 0x00161ED8
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x00163AFC File Offset: 0x00161EFC
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.thingClass == typeof(Building_Art);
		}
	}
}
