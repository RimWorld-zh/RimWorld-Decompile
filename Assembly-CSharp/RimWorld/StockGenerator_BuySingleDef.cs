using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000770 RID: 1904
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		// Token: 0x06002A0D RID: 10765 RVA: 0x00163C0C File Offset: 0x0016200C
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x00163C30 File Offset: 0x00162030
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x040016B0 RID: 5808
		public ThingDef thingDef = null;
	}
}
