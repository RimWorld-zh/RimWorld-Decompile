using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B67 RID: 2919
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x06003FBB RID: 16315 RVA: 0x00219810 File Offset: 0x00217C10
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

		// Token: 0x06003FBC RID: 16316 RVA: 0x00219848 File Offset: 0x00217C48
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(new object[]
			{
				ing.GetBaseCount()
			}) + " (" + ing.filter.Summary + ")";
		}
	}
}
