using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099E RID: 2462
	public class SpecialThingFilterWorker_DeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600373E RID: 14142 RVA: 0x001D8D5C File Offset: 0x001D715C
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && apparel.WornByCorpse;
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x001D8D8C File Offset: 0x001D718C
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
