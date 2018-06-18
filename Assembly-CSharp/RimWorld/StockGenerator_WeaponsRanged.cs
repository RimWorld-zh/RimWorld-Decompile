using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000778 RID: 1912
	public class StockGenerator_WeaponsRanged : StockGenerator_MiscItems
	{
		// Token: 0x06002A30 RID: 10800 RVA: 0x00165CB4 File Offset: 0x001640B4
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.IsRangedWeapon;
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x00165CE0 File Offset: 0x001640E0
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_WeaponsRanged.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x040016C2 RID: 5826
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
	}
}
