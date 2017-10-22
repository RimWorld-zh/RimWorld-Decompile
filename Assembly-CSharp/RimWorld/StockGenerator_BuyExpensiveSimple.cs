using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		public float minValuePerUnit = 15f;

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			yield break;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && thingDef.BaseMarketValue / thingDef.VolumePerUnit > this.minValuePerUnit;
		}
	}
}
