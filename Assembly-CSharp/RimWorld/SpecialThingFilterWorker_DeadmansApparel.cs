using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A0 RID: 2464
	public class SpecialThingFilterWorker_DeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600373F RID: 14143 RVA: 0x001D8678 File Offset: 0x001D6A78
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && apparel.WornByCorpse;
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x001D86A8 File Offset: 0x001D6AA8
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
