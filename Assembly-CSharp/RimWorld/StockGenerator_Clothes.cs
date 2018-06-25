using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000778 RID: 1912
	public class StockGenerator_Clothes : StockGenerator_MiscItems
	{
		// Token: 0x040016C3 RID: 5827
		private static readonly SimpleCurve SelectionWeightMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(500f, 0.5f),
				true
			},
			{
				new CurvePoint(1500f, 0.2f),
				true
			},
			{
				new CurvePoint(5000f, 0.1f),
				true
			}
		};

		// Token: 0x06002A35 RID: 10805 RVA: 0x001661D0 File Offset: 0x001645D0
		public override bool HandlesThingDef(ThingDef td)
		{
			return td != ThingDefOf.Apparel_ShieldBelt && (base.HandlesThingDef(td) && td.IsApparel) && (td.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null) < 0.15f || td.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null) < 0.15f);
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x00166240 File Offset: 0x00164640
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Clothes.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}
	}
}
