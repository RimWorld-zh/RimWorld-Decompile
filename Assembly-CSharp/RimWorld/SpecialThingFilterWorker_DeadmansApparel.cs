using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099C RID: 2460
	public class SpecialThingFilterWorker_DeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600373A RID: 14138 RVA: 0x001D8948 File Offset: 0x001D6D48
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && apparel.WornByCorpse;
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x001D8978 File Offset: 0x001D6D78
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
