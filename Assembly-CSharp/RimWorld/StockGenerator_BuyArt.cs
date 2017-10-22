using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StockGenerator_BuyArt : StockGenerator
	{
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.thingClass == typeof(Building_Art);
		}
	}
}
