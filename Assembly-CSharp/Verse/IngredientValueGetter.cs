using System;

namespace Verse
{
	// Token: 0x02000B65 RID: 2917
	public abstract class IngredientValueGetter
	{
		// Token: 0x06003FB0 RID: 16304
		public abstract float ValuePerUnitOf(ThingDef t);

		// Token: 0x06003FB1 RID: 16305
		public abstract string BillRequirementsDescription(RecipeDef r, IngredientCount ing);

		// Token: 0x06003FB2 RID: 16306 RVA: 0x002194D4 File Offset: 0x002178D4
		public virtual string ExtraDescriptionLine(RecipeDef r)
		{
			return null;
		}
	}
}
