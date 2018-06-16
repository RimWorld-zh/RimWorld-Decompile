using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200077B RID: 1915
	public class StockGenerator_Art : StockGenerator_MiscItems
	{
		// Token: 0x06002A3A RID: 10810 RVA: 0x00165F28 File Offset: 0x00164328
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.Minifiable && td.category == ThingCategory.Building && td.thingClass == typeof(Building_Art);
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x00165F78 File Offset: 0x00164378
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Art.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x040016C6 RID: 5830
		private static readonly SimpleCurve SelectionWeightMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(500f, 1f),
				true
			},
			{
				new CurvePoint(1000f, 0.2f),
				true
			}
		};
	}
}
