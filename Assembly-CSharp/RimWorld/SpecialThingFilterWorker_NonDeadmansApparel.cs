using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099D RID: 2461
	public class SpecialThingFilterWorker_NonDeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600373B RID: 14139 RVA: 0x001D8A1C File Offset: 0x001D6E1C
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && !apparel.WornByCorpse;
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x001D8A50 File Offset: 0x001D6E50
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
