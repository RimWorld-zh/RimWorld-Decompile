using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A0 RID: 2464
	public class SpecialThingFilterWorker_DeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x06003741 RID: 14145 RVA: 0x001D874C File Offset: 0x001D6B4C
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && apparel.WornByCorpse;
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x001D877C File Offset: 0x001D6B7C
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
