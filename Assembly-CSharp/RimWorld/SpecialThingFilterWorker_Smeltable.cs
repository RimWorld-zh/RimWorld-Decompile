using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099D RID: 2461
	public class SpecialThingFilterWorker_Smeltable : SpecialThingFilterWorker
	{
		// Token: 0x06003736 RID: 14134 RVA: 0x001D8558 File Offset: 0x001D6958
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.Smeltable;
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x001D858C File Offset: 0x001D698C
		public override bool CanEverMatch(ThingDef def)
		{
			return def.smeltable;
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x001D85A8 File Offset: 0x001D69A8
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.smeltable && !def.MadeFromStuff;
		}
	}
}
