using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076E RID: 1902
	public class StockGenerator_MultiDef : StockGenerator
	{
		// Token: 0x06002A0E RID: 10766 RVA: 0x001643B0 File Offset: 0x001627B0
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			ThingDef td = this.thingDefs.RandomElement<ThingDef>();
			foreach (Thing th in StockGeneratorUtility.TryMakeForStock(td, base.RandomCountOf(td)))
			{
				yield return th;
			}
			yield break;
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x001643DC File Offset: 0x001627DC
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x00164400 File Offset: 0x00162800
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return e;
			}
			for (int i = 0; i < this.thingDefs.Count; i++)
			{
				if (!this.thingDefs[i].tradeability.TraderCanSell())
				{
					yield return this.thingDefs[i] + " tradeability doesn't allow traders to sell this thing";
				}
			}
			yield break;
		}

		// Token: 0x040016B0 RID: 5808
		private List<ThingDef> thingDefs = new List<ThingDef>();
	}
}
