using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000776 RID: 1910
	public class StockGenerator_WeaponsRanged : StockGenerator_MiscItems
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
				new CurvePoint(1500f, 0.2f),
				true
			},
			{
				new CurvePoint(5000f, 0.1f),
				true
			}
		};

		// Token: 0x06002A2C RID: 10796 RVA: 0x0016623C File Offset: 0x0016463C
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.IsRangedWeapon;
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x00166268 File Offset: 0x00164668
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_WeaponsRanged.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}
	}
}
