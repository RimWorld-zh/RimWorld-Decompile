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

		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(new object[]
			{
				ing.GetBaseCount()
			}) + " (" + ing.filter.Summary + ")";
		}
	}
}
