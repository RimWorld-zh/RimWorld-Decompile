using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000B66 RID: 2918
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		// Token: 0x06003FB4 RID: 16308 RVA: 0x002194F4 File Offset: 0x002178F4
		public override float ValuePerUnitOf(ThingDef t)
		{
			float result;
			if (t.IsStuff)
			{
				result = t.VolumePerUnit;
			}
			else
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x00219528 File Offset: 0x00217928
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			string result;
			if (!ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume) || ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)))
			{
				result = "BillRequires".Translate(new object[]
				{
					ing.GetBaseCount(),
					ing.filter.Summary
				});
			}
			else
			{
				result = "BillRequires".Translate(new object[]
				{
					ing.GetBaseCount() * 10f,
					ing.filter.Summary
				});
			}
			return result;
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x00219604 File Offset: 0x00217A04
		public override string ExtraDescriptionLine(RecipeDef r)
		{
			string result;
			if (r.ingredients.Any((IngredientCount ing) => ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td))))
			{
				result = "BillRequiresMayVary".Translate(new object[]
				{
					10.ToStringCached()
				});
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
