using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000770 RID: 1904
	public class StockGenerator_Tag : StockGenerator
	{
		// Token: 0x06002A16 RID: 10774 RVA: 0x00164DFC File Offset: 0x001631FC
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

		// Token: 0x06002A17 RID: 10775 RVA: 0x00164E28 File Offset: 0x00163228
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}

		// Token: 0x040016B5 RID: 5813
		[NoTranslate]
		private string tradeTag = null;

		// Token: 0x040016B6 RID: 5814
		private IntRange thingDefCountRange = IntRange.one;

		// Token: 0x040016B7 RID: 5815
		private List<ThingDef> excludedThingDefs = null;
	}
}
