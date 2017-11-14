using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class StockGenerator_Tag : StockGenerator
	{
		private string tradeTag;

		private IntRange thingDefCountRange = IntRange.one;

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			_003CGenerateThings_003Ec__Iterator0 _003CGenerateThings_003Ec__Iterator = (_003CGenerateThings_003Ec__Iterator0)/*Error near IL_0034: stateMachine*/;
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			int i = 0;
			ThingDef chosenThingDef;
			while (i < numThingDefsToUse && (from d in DefDatabase<ThingDef>.AllDefs
			where _003CGenerateThings_003Ec__Iterator._0024this.HandlesThingDef(d) && d.tradeability == Tradeability.Stockable && !generatedDefs.Contains(d)
			select d).TryRandomElement<ThingDef>(out chosenThingDef))
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
			IL_0178:
			/*Error near IL_0179: Unexpected return in MoveNext()*/;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != 0 && (int)thingDef.techLevel <= (int)base.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}
	}
}
