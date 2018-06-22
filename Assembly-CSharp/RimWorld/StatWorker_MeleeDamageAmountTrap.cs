using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C4 RID: 2500
	public class StatWorker_MeleeDamageAmountTrap : StatWorker_MeleeDamageAmount
	{
		// Token: 0x06003815 RID: 14357 RVA: 0x001DE7B4 File Offset: 0x001DCBB4
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isTrap;
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x001DE7F8 File Offset: 0x001DCBF8
		protected override DamageArmorCategoryDef CategoryOfDamage(ThingDef def)
		{
			return def.building.trapDamageCategory;
		}
	}
}
