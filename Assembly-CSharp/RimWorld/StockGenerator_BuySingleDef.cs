using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000770 RID: 1904
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		// Token: 0x06002A0B RID: 10763 RVA: 0x00163B78 File Offset: 0x00161F78
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x00163B9C File Offset: 0x00161F9C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x040016B0 RID: 5808
		public ThingDef thingDef = null;
	}
}
