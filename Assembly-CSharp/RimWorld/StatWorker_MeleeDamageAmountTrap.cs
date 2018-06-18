using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C8 RID: 2504
	public class StatWorker_MeleeDamageAmountTrap : StatWorker_MeleeDamageAmount
	{
		// Token: 0x0600381B RID: 14363 RVA: 0x001DE5DC File Offset: 0x001DC9DC
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isTrap;
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x001DE620 File Offset: 0x001DCA20
		protected override DamageArmorCategoryDef CategoryOfDamage(ThingDef def)
		{
			return def.building.trapDamageCategory;
		}
	}
}
