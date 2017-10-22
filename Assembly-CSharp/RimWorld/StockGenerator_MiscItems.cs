using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			int count = base.countRange.RandomInRange;
			int i = 0;
			ThingDef finalDef;
			while (i < count && (from t in DefDatabase<ThingDef>.AllDefs
			where ((_003CGenerateThings_003Ec__Iterator180)/*Error near IL_0048: stateMachine*/)._003C_003Ef__this.HandlesThingDef(t) && (int)t.techLevel <= (int)((_003CGenerateThings_003Ec__Iterator180)/*Error near IL_0048: stateMachine*/)._003C_003Ef__this.maxTechLevelGenerate
			select t).TryRandomElementByWeight<ThingDef>(new Func<ThingDef, float>(this.SelectionWeight), out finalDef))
			{
				yield return this.MakeThing(finalDef);
				i++;
			}
		}

		protected virtual Thing MakeThing(ThingDef def)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1);
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability == Tradeability.Stockable && (int)thingDef.techLevel <= (int)base.maxTechLevelBuy;
		}

		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
