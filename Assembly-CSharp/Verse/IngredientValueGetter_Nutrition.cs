using System;
using RimWorld;

namespace Verse
{
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		public IngredientValueGetter_Nutrition()
		{
		}

		public override float ValuePerUnitOf(ThingDef t)
		{
			if (!t.IsNutritionGivingIngestible)
			{
				return 0f;
			}
			return t.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(new object[]
			{
				ing.GetBaseCount()
			}) + " (" + ing.filter.Summary + ")";
		}
	}
}
