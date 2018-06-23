using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000777 RID: 1911
	public class StockGenerator_Art : StockGenerator_MiscItems
	{
		// Token: 0x040016C4 RID: 5828
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

		// Token: 0x06002A35 RID: 10805 RVA: 0x00166194 File Offset: 0x00164594
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.Minifiable && td.category == ThingCategory.Building && td.thingClass == typeof(Building_Art);
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x001661E4 File Offset: 0x001645E4
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Art.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}
	}
}
