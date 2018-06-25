using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000779 RID: 1913
	public class StockGenerator_Art : StockGenerator_MiscItems
	{
		// Token: 0x040016C8 RID: 5832
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

		// Token: 0x06002A38 RID: 10808 RVA: 0x00166544 File Offset: 0x00164944
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.Minifiable && td.category == ThingCategory.Building && td.thingClass == typeof(Building_Art);
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x00166594 File Offset: 0x00164994
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Art.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}
	}
}
