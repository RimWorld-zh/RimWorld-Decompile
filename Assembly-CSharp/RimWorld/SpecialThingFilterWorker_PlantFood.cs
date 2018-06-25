using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099A RID: 2458
	public class SpecialThingFilterWorker_PlantFood : SpecialThingFilterWorker
	{
		// Token: 0x0600372F RID: 14127 RVA: 0x001D8AE8 File Offset: 0x001D6EE8
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x001D8B0C File Offset: 0x001D6F0C
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.ingestible != null && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.None;
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x001D8B44 File Offset: 0x001D6F44
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
