using System;

namespace Verse
{
	// Token: 0x02000B61 RID: 2913
	public abstract class IngredientValueGetter
	{
		// Token: 0x06003FB3 RID: 16307
		public abstract float ValuePerUnitOf(ThingDef t);

		// Token: 0x06003FB4 RID: 16308
		public abstract string BillRequirementsDescription(RecipeDef r, IngredientCount ing);

		// Token: 0x06003FB5 RID: 16309 RVA: 0x00219C10 File Offset: 0x00218010
		public virtual string ExtraDescriptionLine(RecipeDef r)
		{
			return null;
		}
	}
}
