using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StockGenerator_MultiDef : StockGenerator
	{
		private List<ThingDef> thingDefs = new List<ThingDef>();

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			ThingDef td = this.thingDefs.RandomElement();
			foreach (Thing item in StockGeneratorUtility.TryMakeForStock(td, base.RandomCountOf(td)))
			{
				yield return item;
			}
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}
	}
}
