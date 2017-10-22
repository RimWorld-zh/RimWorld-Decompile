using System;
using System.Collections.Generic;
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

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			int i = 0;
			_003CGenerateThings_003Ec__Iterator17C _003CGenerateThings_003Ec__Iterator17C;
			ThingDef chosenThingDef;
			while (i < numThingDefsToUse && this.categoryDef.DescendantThingDefs.Where((Func<ThingDef, bool>)delegate(ThingDef t)
			{
				_003CGenerateThings_003Ec__Iterator17C = (_003CGenerateThings_003Ec__Iterator17C)/*Error near IL_0060: stateMachine*/;
				return t.tradeability == Tradeability.Stockable && (int)t.techLevel <= (int)((_003CGenerateThings_003Ec__Iterator17C)/*Error near IL_0060: stateMachine*/)._003C_003Ef__this.maxTechLevelGenerate && !((_003CGenerateThings_003Ec__Iterator17C)/*Error near IL_0060: stateMachine*/)._003CgeneratedDefs_003E__0.Contains(t) && (((_003CGenerateThings_003Ec__Iterator17C)/*Error near IL_0060: stateMachine*/)._003C_003Ef__this.excludedThingDefs == null || !((_003CGenerateThings_003Ec__Iterator17C)/*Error near IL_0060: stateMachine*/)._003C_003Ef__this.excludedThingDefs.Contains(t)) && (((_003CGenerateThings_003Ec__Iterator17C)/*Error near IL_0060: stateMachine*/)._003C_003Ef__this.excludedCategories == null || !((_003CGenerateThings_003Ec__Iterator17C)/*Error near IL_0060: stateMachine*/)._003C_003Ef__this.excludedCategories.Any((Predicate<ThingCategoryDef>)((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t))));
			}).TryRandomElement<ThingDef>(out chosenThingDef))
			{
				foreach (Thing item in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)))
				{
					yield return item;
				}
				generatedDefs.Add(chosenThingDef);
				i++;
			}
		}

		public override bool HandlesThingDef(ThingDef t)
		{
			return this.categoryDef.DescendantThingDefs.Contains(t) && (int)t.techLevel <= (int)base.maxTechLevelBuy && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((Predicate<ThingCategoryDef>)((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t))));
		}
	}
}
