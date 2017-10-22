using System;
using System.Linq;

namespace Verse
{
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		public override float ValuePerUnitOf(ThingDef t)
		{
			return (float)((!t.IsStuff) ? 1.0 : t.VolumePerUnit);
		}

		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return (ing.filter.AllowedThingDefs.Any((Func<ThingDef, bool>)((ThingDef td) => td.smallVolume)) && !ing.filter.AllowedThingDefs.Any((Func<ThingDef, bool>)((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)))) ? "BillRequires".Translate((float)(ing.GetBaseCount() * 10.0), ing.filter.Summary) : "BillRequires".Translate(ing.GetBaseCount(), ing.filter.Summary);
		}

		public override string ExtraDescriptionLine(RecipeDef r)
		{
			return (!r.ingredients.Any((Predicate<IngredientCount>)((IngredientCount ing) => ing.filter.AllowedThingDefs.Any((Func<ThingDef, bool>)((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)))))) ? null : "BillRequiresMayVary".Translate(10.ToStringCached());
		}
	}
}
