using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B67 RID: 2919
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x06003FB9 RID: 16313 RVA: 0x0021973C File Offset: 0x00217B3C
		public override float ValuePerUnitOf(ThingDef t)
		{
			float result;
			if (!t.IsNutritionGivingIngestible)
			{
				result = 0f;
			}
			else
			{
				result = t.GetStatValueAbstract(StatDefOf.Nutrition, null);
			}
			return result;
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x00219774 File Offset: 0x00217B74
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(new object[]
			{
				ing.GetBaseCount()
			}) + " (" + ing.filter.Summary + ")";
		}
	}
}
