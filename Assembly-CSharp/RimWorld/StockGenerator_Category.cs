using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class StockGenerator_Category : StockGenerator
	{
		private ThingCategoryDef categoryDef;

		private IntRange thingDefCountRange = IntRange.one;

		private List<ThingDef> excludedThingDefs;

		private List<ThingCategoryDef> excludedCategories;

		[DebuggerHidden]
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			StockGenerator_Category.<GenerateThings>c__Iterator17B <GenerateThings>c__Iterator17B = new StockGenerator_Category.<GenerateThings>c__Iterator17B();
			<GenerateThings>c__Iterator17B.<>f__this = this;
			StockGenerator_Category.<GenerateThings>c__Iterator17B expr_0E = <GenerateThings>c__Iterator17B;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override bool HandlesThingDef(ThingDef t)
		{
			return this.categoryDef.DescendantThingDefs.Contains(t) && t.techLevel <= this.maxTechLevelBuy && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
		}
	}
}
