using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C6 RID: 2502
	public class StatWorker_MeleeDamageAmountTrap : StatWorker_MeleeDamageAmount
	{
		// Token: 0x06003819 RID: 14361 RVA: 0x001DEBCC File Offset: 0x001DCFCC
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isTrap;
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x001DEC10 File Offset: 0x001DD010
		protected override DamageArmorCategoryDef CategoryOfDamage(ThingDef def)
		{
			return def.building.trapDamageCategory;
		}
	}
}
