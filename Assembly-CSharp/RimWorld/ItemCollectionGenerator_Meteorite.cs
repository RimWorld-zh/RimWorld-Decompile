using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_Meteorite : ItemCollectionGenerator
	{
		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		private const float PreciousMineableMarketValue = 5f;

		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			ItemCollectionGeneratorParams parms2 = parms;
			int? count = parms.count;
			parms2.count = new int?((!count.HasValue) ? ItemCollectionGenerator_Meteorite.MineablesCountRange.RandomInRange : count.Value);
			parms2.extraAllowedDefs = Gen.YieldSingle(this.FindRandomMineableDef());
			outThings.AddRange(ItemCollectionGeneratorDefOf.Standard.Worker.Generate(parms2));
		}

		private ThingDef FindRandomMineableDef()
		{
			float value = Rand.Value;
			IEnumerable<ThingDef> source = from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.mineable && x != ThingDefOf.CollapsedRocks
			select x;
			return (!(value < 0.40000000596046448)) ? ((!(value < 0.800000011920929)) ? (from x in source
			where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue >= 5.0
			select x).RandomElement() : (from x in source
			where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue < 5.0
			select x).RandomElement()) : (from x in source
			where !x.building.isResourceRock
			select x).RandomElement();
		}
	}
}
