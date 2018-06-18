using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000772 RID: 1906
	public class StockGenerator_MultiDef : StockGenerator
	{
		// Token: 0x06002A15 RID: 10773 RVA: 0x001641D8 File Offset: 0x001625D8
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			ThingDef td = this.thingDefs.RandomElement<ThingDef>();
			foreach (Thing th in StockGeneratorUtility.TryMakeForStock(td, base.RandomCountOf(td)))
			{
				yield return th;
			}
			yield break;
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x00164204 File Offset: 0x00162604
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x00164228 File Offset: 0x00162628
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

		// Token: 0x040016B2 RID: 5810
		private List<ThingDef> thingDefs = new List<ThingDef>();
	}
}
