using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B65 RID: 2917
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x06003FBF RID: 16319 RVA: 0x00219F54 File Offset: 0x00218354
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

		// Token: 0x06003FC0 RID: 16320 RVA: 0x00219F8C File Offset: 0x0021838C
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(new object[]
			{
				ing.GetBaseCount()
			}) + " (" + ing.filter.Summary + ")";
		}
	}
}
