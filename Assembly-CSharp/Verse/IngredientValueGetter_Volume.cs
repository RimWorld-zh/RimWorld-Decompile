using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse
{
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		public IngredientValueGetter_Volume()
		{
		}

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

		[CompilerGenerated]
		private static bool <BillRequirementsDescription>m__0(ThingDef td)
		{
			return td.smallVolume;
		}

		[CompilerGenerated]
		private sealed class <BillRequirementsDescription>c__AnonStorey0
		{
			internal RecipeDef r;

			public <BillRequirementsDescription>c__AnonStorey0()
			{
			}

			internal bool <>m__0(ThingDef td)
			{
				return td.smallVolume && !this.r.GetPremultipliedSmallIngredients().Contains(td);
			}
		}

		[CompilerGenerated]
		private sealed class <ExtraDescriptionLine>c__AnonStorey1
		{
			internal RecipeDef r;

			public <ExtraDescriptionLine>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IngredientCount ing)
			{
				return ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume && !this.r.GetPremultipliedSmallIngredients().Contains(td));
			}

			internal bool <>m__1(ThingDef td)
			{
				return td.smallVolume && !this.r.GetPremultipliedSmallIngredients().Contains(td);
			}
		}
	}
}
