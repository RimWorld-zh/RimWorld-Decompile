using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B63 RID: 2915
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x06003FBC RID: 16316 RVA: 0x00219E78 File Offset: 0x00218278
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

		// Token: 0x06003FBD RID: 16317 RVA: 0x00219EB0 File Offset: 0x002182B0
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(new object[]
			{
				ing.GetBaseCount()
			}) + " (" + ing.filter.Summary + ")";
		}
	}
}
