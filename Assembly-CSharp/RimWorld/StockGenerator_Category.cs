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
			_003CGenerateThings_003Ec__Iterator0 _003CGenerateThings_003Ec__Iterator = (_003CGenerateThings_003Ec__Iterator0)/*Error near IL_0034: stateMachine*/;
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			int i = 0;
			ThingDef chosenThingDef;
			while (i < numThingDefsToUse && this.categoryDef.DescendantThingDefs.Where(delegate(ThingDef t)
			{
				_003CGenerateThings_003Ec__Iterator0 _003CGenerateThings_003Ec__Iterator2 = _003CGenerateThings_003Ec__Iterator;
				return t.tradeability == Tradeability.Stockable && (int)t.techLevel <= (int)_003CGenerateThings_003Ec__Iterator._0024this.maxTechLevelGenerate && !generatedDefs.Contains(t) && (_003CGenerateThings_003Ec__Iterator._0024this.excludedThingDefs == null || !_003CGenerateThings_003Ec__Iterator._0024this.excludedThingDefs.Contains(t)) && (_003CGenerateThings_003Ec__Iterator._0024this.excludedCategories == null || !_003CGenerateThings_003Ec__Iterator._0024this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
			}).TryRandomElement<ThingDef>(out chosenThingDef))
			{
				using (IEnumerator<Thing> enumerator = StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Thing th = enumerator.Current;
						yield return th;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				generatedDefs.Add(chosenThingDef);
				i++;
			}
			yield break;
			IL_0183:
			/*Error near IL_0184: Unexpected return in MoveNext()*/;
		}

		public override bool HandlesThingDef(ThingDef t)
		{
			return this.categoryDef.DescendantThingDefs.Contains(t) && t.tradeability != 0 && (int)t.techLevel <= (int)base.maxTechLevelBuy && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
		}
	}
}
