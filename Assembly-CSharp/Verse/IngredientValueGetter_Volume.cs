using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000B62 RID: 2914
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		// Token: 0x06003FB7 RID: 16311 RVA: 0x00219C30 File Offset: 0x00218030
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

		// Token: 0x06003FB8 RID: 16312 RVA: 0x00219C64 File Offset: 0x00218064
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

		// Token: 0x06003FB9 RID: 16313 RVA: 0x00219D40 File Offset: 0x00218140
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
