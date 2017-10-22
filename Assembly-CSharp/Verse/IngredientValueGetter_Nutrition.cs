namespace Verse
{
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		public override float ValuePerUnitOf(ThingDef t)
		{
			return (float)(t.IsNutritionGivingIngestible ? t.ingestible.nutrition : 0.0);
		}

		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(ing.GetBaseCount()) + " (" + ing.filter.Summary + ")";
		}
	}
}
