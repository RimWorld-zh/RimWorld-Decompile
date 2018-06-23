using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000775 RID: 1909
	public class StockGenerator_Armor : StockGenerator_MiscItems
	{
		// Token: 0x040016C1 RID: 5825
		public const float MinArmor = 0.15f;

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

		// Token: 0x06002A2D RID: 10797 RVA: 0x00165F5C File Offset: 0x0016435C
		public override bool HandlesThingDef(ThingDef td)
		{
			return td == ThingDefOf.Apparel_ShieldBelt || td == ThingDefOf.Apparel_SmokepopBelt || (base.HandlesThingDef(td) && td.IsApparel && (td.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null) > 0.15f || td.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null) > 0.15f));
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x00165FDC File Offset: 0x001643DC
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Armor.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}
	}
}
