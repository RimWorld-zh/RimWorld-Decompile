using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C8 RID: 2504
	public class StatWorker_MeleeDamageAmountTrap : StatWorker_MeleeDamageAmount
	{
		// Token: 0x06003819 RID: 14361 RVA: 0x001DE508 File Offset: 0x001DC908
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isTrap;
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x001DE54C File Offset: 0x001DC94C
		protected override DamageArmorCategoryDef CategoryOfDamage(ThingDef def)
		{
			return def.building.trapDamageCategory;
		}
	}
}
