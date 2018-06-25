using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000771 RID: 1905
	public class StockGenerator_Category : StockGenerator
	{
		// Token: 0x040016B1 RID: 5809
		private ThingCategoryDef categoryDef = null;

		// Token: 0x040016B2 RID: 5810
		private IntRange thingDefCountRange = IntRange.one;

		// Token: 0x040016B3 RID: 5811
		private List<ThingDef> excludedThingDefs = null;

		// Token: 0x040016B4 RID: 5812
		private List<ThingCategoryDef> excludedCategories = null;

		// Token: 0x06002A17 RID: 10775 RVA: 0x00164A50 File Offset: 0x00162E50
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			for (int i = 0; i < numThingDefsToUse; i++)
			{
				ThingDef chosenThingDef;
				if (!(from t in this.categoryDef.DescendantThingDefs
				where t.tradeability.TraderCanSell() && t.techLevel <= this.maxTechLevelGenerate && !generatedDefs.Contains(t) && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)))
				select t).TryRandomElement(out chosenThingDef))
				{
					break;
				}
				foreach (Thing th in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)))
				{
					yield return th;
				}
				generatedDefs.Add(chosenThingDef);
			}
			yield break;
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x00164A7C File Offset: 0x00162E7C
		public override bool HandlesThingDef(ThingDef t)
		{
			return this.categoryDef.DescendantThingDefs.Contains(t) && t.tradeability != Tradeability.None && t.techLevel <= this.maxTechLevelBuy && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
		}
	}
}
