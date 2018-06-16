using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099C RID: 2460
	public class SpecialThingFilterWorker_PlantFood : SpecialThingFilterWorker
	{
		// Token: 0x06003730 RID: 14128 RVA: 0x001D8404 File Offset: 0x001D6804
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x001D8428 File Offset: 0x001D6828
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.ingestible != null && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.None;
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x001D8460 File Offset: 0x001D6860
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
