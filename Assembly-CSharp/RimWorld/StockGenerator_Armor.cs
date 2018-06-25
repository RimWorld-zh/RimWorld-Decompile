using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000777 RID: 1911
	public class StockGenerator_Armor : StockGenerator_MiscItems
	{
		// Token: 0x040016C5 RID: 5829
		public const float MinArmor = 0.15f;

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
				new CurvePoint(1500f, 0.2f),
				true
			},
			{
				new CurvePoint(5000f, 0.1f),
				true
			}
		};

		// Token: 0x06002A30 RID: 10800 RVA: 0x0016630C File Offset: 0x0016470C
		public override bool HandlesThingDef(ThingDef td)
		{
			return td == ThingDefOf.Apparel_ShieldBelt || td == ThingDefOf.Apparel_SmokepopBelt || (base.HandlesThingDef(td) && td.IsApparel && (td.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null) > 0.15f || td.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null) > 0.15f));
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x0016638C File Offset: 0x0016478C
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Armor.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}
	}
}
