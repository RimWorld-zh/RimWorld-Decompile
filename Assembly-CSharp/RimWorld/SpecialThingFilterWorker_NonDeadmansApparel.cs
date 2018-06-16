using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099F RID: 2463
	public class SpecialThingFilterWorker_NonDeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600373C RID: 14140 RVA: 0x001D860C File Offset: 0x001D6A0C
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && !apparel.WornByCorpse;
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x001D8640 File Offset: 0x001D6A40
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
