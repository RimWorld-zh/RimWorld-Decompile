using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200077B RID: 1915
	public class StockGenerator_Art : StockGenerator_MiscItems
	{
		// Token: 0x06002A3C RID: 10812 RVA: 0x00165FBC File Offset: 0x001643BC
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.Minifiable && td.category == ThingCategory.Building && td.thingClass == typeof(Building_Art);
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x0016600C File Offset: 0x0016440C
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
