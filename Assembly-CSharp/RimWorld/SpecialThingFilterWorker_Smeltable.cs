using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000999 RID: 2457
	public class SpecialThingFilterWorker_Smeltable : SpecialThingFilterWorker
	{
		// Token: 0x0600372F RID: 14127 RVA: 0x001D8754 File Offset: 0x001D6B54
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.Smeltable;
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x001D8788 File Offset: 0x001D6B88
		public override bool CanEverMatch(ThingDef def)
		{
			return def.smeltable;
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x001D87A4 File Offset: 0x001D6BA4
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.smeltable && !def.MadeFromStuff;
		}
	}
}
