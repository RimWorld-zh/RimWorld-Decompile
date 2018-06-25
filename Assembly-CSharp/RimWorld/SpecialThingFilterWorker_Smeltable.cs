using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099B RID: 2459
	public class SpecialThingFilterWorker_Smeltable : SpecialThingFilterWorker
	{
		// Token: 0x06003733 RID: 14131 RVA: 0x001D8894 File Offset: 0x001D6C94
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.Smeltable;
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x001D88C8 File Offset: 0x001D6CC8
		public override bool CanEverMatch(ThingDef def)
		{
			return def.smeltable;
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x001D88E4 File Offset: 0x001D6CE4
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.smeltable && !def.MadeFromStuff;
		}
	}
}
