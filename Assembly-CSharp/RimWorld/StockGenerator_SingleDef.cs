using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076F RID: 1903
	public class StockGenerator_SingleDef : StockGenerator
	{
		// Token: 0x040016AF RID: 5807
		private ThingDef thingDef = null;

		// Token: 0x06002A0D RID: 10765 RVA: 0x00164020 File Offset: 0x00162420
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			foreach (Thing th in StockGeneratorUtility.TryMakeForStock(this.thingDef, base.RandomCountOf(this.thingDef)))
			{
				yield return th;
			}
			yield break;
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x0016404C File Offset: 0x0016244C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x0016406C File Offset: 0x0016246C
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return e;
			}
			if (!this.thingDef.tradeability.TraderCanSell())
			{
				yield return this.thingDef + " tradeability doesn't allow traders to sell this thing";
			}
			yield break;
		}
	}
}
