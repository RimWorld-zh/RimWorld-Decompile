using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000771 RID: 1905
	public class StockGenerator_SingleDef : StockGenerator
	{
		// Token: 0x06002A0E RID: 10766 RVA: 0x00163C64 File Offset: 0x00162064
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			foreach (Thing th in StockGeneratorUtility.TryMakeForStock(this.thingDef, base.RandomCountOf(this.thingDef)))
			{
				yield return th;
			}
			yield break;
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x00163C90 File Offset: 0x00162090
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x00163CB0 File Offset: 0x001620B0
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

		// Token: 0x040016B1 RID: 5809
		private ThingDef thingDef = null;
	}
}
