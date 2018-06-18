using System;

namespace Verse
{
	// Token: 0x02000B65 RID: 2917
	public abstract class IngredientValueGetter
	{
		// Token: 0x06003FB2 RID: 16306
		public abstract float ValuePerUnitOf(ThingDef t);

		// Token: 0x06003FB3 RID: 16307
		public abstract string BillRequirementsDescription(RecipeDef r, IngredientCount ing);

		// Token: 0x06003FB4 RID: 16308 RVA: 0x002195A8 File Offset: 0x002179A8
		public virtual string ExtraDescriptionLine(RecipeDef r)
		{
			return null;
		}
	}
}
