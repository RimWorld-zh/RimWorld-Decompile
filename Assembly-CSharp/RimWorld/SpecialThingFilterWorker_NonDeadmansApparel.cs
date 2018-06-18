using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099F RID: 2463
	public class SpecialThingFilterWorker_NonDeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600373E RID: 14142 RVA: 0x001D86E0 File Offset: 0x001D6AE0
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && !apparel.WornByCorpse;
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x001D8714 File Offset: 0x001D6B14
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
