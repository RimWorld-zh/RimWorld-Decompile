using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000774 RID: 1908
	public class StockGenerator_WeaponsRanged : StockGenerator_MiscItems
	{
		// Token: 0x040016C0 RID: 5824
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
				new CurvePoint(1500f, 0.2f),
				true
			},
			{
				new CurvePoint(5000f, 0.1f),
				true
			}
		};

		// Token: 0x06002A29 RID: 10793 RVA: 0x00165E8C File Offset: 0x0016428C
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.IsRangedWeapon;
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x00165EB8 File Offset: 0x001642B8
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_WeaponsRanged.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}
	}
}
