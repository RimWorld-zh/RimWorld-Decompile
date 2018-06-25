using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000772 RID: 1906
	public class StockGenerator_Tag : StockGenerator
	{
		// Token: 0x040016B9 RID: 5817
		[NoTranslate]
		private string tradeTag = null;

		// Token: 0x040016BA RID: 5818
		private IntRange thingDefCountRange = IntRange.one;

		// Token: 0x040016BB RID: 5819
		private List<ThingDef> excludedThingDefs = null;

		// Token: 0x06002A19 RID: 10777 RVA: 0x001651AC File Offset: 0x001635AC
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			for (int i = 0; i < numThingDefsToUse; i++)
			{
				ThingDef chosenThingDef;
				if (!(from d in DefDatabase<ThingDef>.AllDefs
				where this.HandlesThingDef(d) && d.tradeability.TraderCanSell() && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(d)) && !generatedDefs.Contains(d)
				select d).TryRandomElement(out chosenThingDef))
				{
					yield break;
				}
				foreach (Thing th in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)))
				{
					yield return th;
				}
				generatedDefs.Add(chosenThingDef);
			}
			yield break;
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x001651D8 File Offset: 0x001635D8
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}
	}
}
