using System;
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
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			int i = 0;
			ThingDef chosenThingDef;
			while (i < numThingDefsToUse && (from d in DefDatabase<ThingDef>.AllDefs
			where ((_003CGenerateThings_003Ec__Iterator17D)/*Error near IL_0055: stateMachine*/)._003C_003Ef__this.HandlesThingDef(d) && !((_003CGenerateThings_003Ec__Iterator17D)/*Error near IL_0055: stateMachine*/)._003CgeneratedDefs_003E__0.Contains(d)
			select d).TryRandomElement<ThingDef>(out chosenThingDef))
			{
				foreach (Thing item in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)))
				{
					yield return item;
				}
				generatedDefs.Add(chosenThingDef);
				i++;
			}
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability == Tradeability.Stockable && (int)thingDef.techLevel <= (int)base.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}
	}
}
