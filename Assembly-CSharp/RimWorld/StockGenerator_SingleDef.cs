using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StockGenerator_SingleDef : StockGenerator
	{
		private ThingDef thingDef;

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			foreach (Thing item in StockGeneratorUtility.TryMakeForStock(this.thingDef, base.RandomCountOf(this.thingDef)))
			{
				yield return item;
			}
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}
	}
}
