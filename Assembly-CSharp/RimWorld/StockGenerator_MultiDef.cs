using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000770 RID: 1904
	public class StockGenerator_MultiDef : StockGenerator
	{
		// Token: 0x040016B0 RID: 5808
		private List<ThingDef> thingDefs = new List<ThingDef>();

		// Token: 0x06002A12 RID: 10770 RVA: 0x00164500 File Offset: 0x00162900
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			ThingDef td = this.thingDefs.RandomElement<ThingDef>();
			foreach (Thing th in StockGeneratorUtility.TryMakeForStock(td, base.RandomCountOf(td)))
			{
				yield return th;
			}
			yield break;
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x0016452C File Offset: 0x0016292C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x00164550 File Offset: 0x00162950
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
	}
}
