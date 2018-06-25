using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099A RID: 2458
	public class SpecialThingFilterWorker_PlantFood : SpecialThingFilterWorker
	{
		// Token: 0x0600372F RID: 14127 RVA: 0x001D8814 File Offset: 0x001D6C14
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x001D8838 File Offset: 0x001D6C38
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.ingestible != null && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.None;
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x001D8870 File Offset: 0x001D6C70
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
