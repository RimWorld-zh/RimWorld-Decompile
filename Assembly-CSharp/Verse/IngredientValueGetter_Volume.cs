using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000B65 RID: 2917
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		// Token: 0x06003FBA RID: 16314 RVA: 0x00219FEC File Offset: 0x002183EC
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

		// Token: 0x06003FBB RID: 16315 RVA: 0x0021A020 File Offset: 0x00218420
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

		// Token: 0x06003FBC RID: 16316 RVA: 0x0021A0FC File Offset: 0x002184FC
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
