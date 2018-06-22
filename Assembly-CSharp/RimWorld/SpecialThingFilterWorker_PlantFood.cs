using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000998 RID: 2456
	public class SpecialThingFilterWorker_PlantFood : SpecialThingFilterWorker
	{
		// Token: 0x0600372B RID: 14123 RVA: 0x001D86D4 File Offset: 0x001D6AD4
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x001D86F8 File Offset: 0x001D6AF8
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.ingestible != null && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.None;
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x001D8730 File Offset: 0x001D6B30
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
