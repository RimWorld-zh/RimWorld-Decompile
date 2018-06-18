using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000779 RID: 1913
	public class StockGenerator_Armor : StockGenerator_MiscItems
	{
		// Token: 0x06002A34 RID: 10804 RVA: 0x00165D84 File Offset: 0x00164184
		public override bool HandlesThingDef(ThingDef td)
		{
			return td == ThingDefOf.Apparel_ShieldBelt || td == ThingDefOf.Apparel_SmokepopBelt || (base.HandlesThingDef(td) && td.IsApparel && (td.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null) > 0.15f || td.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null) > 0.15f));
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x00165E04 File Offset: 0x00164204
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Armor.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x040016C3 RID: 5827
		public const float MinArmor = 0.15f;

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
	}
}
