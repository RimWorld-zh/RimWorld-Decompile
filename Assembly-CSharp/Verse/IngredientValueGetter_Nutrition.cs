using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B66 RID: 2918
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x06003FBF RID: 16319 RVA: 0x0021A234 File Offset: 0x00218634
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

		// Token: 0x06003FC0 RID: 16320 RVA: 0x0021A26C File Offset: 0x0021866C
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(new object[]
			{
				ing.GetBaseCount()
			}) + " (" + ing.filter.Summary + ")";
		}
	}
}
