using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076D RID: 1901
	public class StockGenerator_SingleDef : StockGenerator
	{
		// Token: 0x06002A09 RID: 10761 RVA: 0x00163ED0 File Offset: 0x001622D0
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			foreach (Thing th in StockGeneratorUtility.TryMakeForStock(this.thingDef, base.RandomCountOf(this.thingDef)))
			{
				yield return th;
			}
			yield break;
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x00163EFC File Offset: 0x001622FC
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x00163F1C File Offset: 0x0016231C
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

		// Token: 0x040016AF RID: 5807
		private ThingDef thingDef = null;
	}
}
