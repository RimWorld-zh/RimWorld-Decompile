using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099B RID: 2459
	public class SpecialThingFilterWorker_NonDeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x06003737 RID: 14135 RVA: 0x001D88DC File Offset: 0x001D6CDC
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && !apparel.WornByCorpse;
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x001D8910 File Offset: 0x001D6D10
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
