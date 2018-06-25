using System;

namespace Verse
{
	// Token: 0x02000B64 RID: 2916
	public abstract class IngredientValueGetter
	{
		// Token: 0x06003FB6 RID: 16310
		public abstract float ValuePerUnitOf(ThingDef t);

		// Token: 0x06003FB7 RID: 16311
		public abstract string BillRequirementsDescription(RecipeDef r, IngredientCount ing);

		// Token: 0x06003FB8 RID: 16312 RVA: 0x00219FCC File Offset: 0x002183CC
		public virtual string ExtraDescriptionLine(RecipeDef r)
		{
			return null;
		}
	}
}
