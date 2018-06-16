using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099D RID: 2461
	public class SpecialThingFilterWorker_Smeltable : SpecialThingFilterWorker
	{
		// Token: 0x06003734 RID: 14132 RVA: 0x001D8484 File Offset: 0x001D6884
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.Smeltable;
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x001D84B8 File Offset: 0x001D68B8
		public override bool CanEverMatch(ThingDef def)
		{
			return def.smeltable;
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x001D84D4 File Offset: 0x001D68D4
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.smeltable && !def.MadeFromStuff;
		}
	}
}
