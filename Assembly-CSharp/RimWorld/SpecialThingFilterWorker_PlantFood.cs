using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099C RID: 2460
	public class SpecialThingFilterWorker_PlantFood : SpecialThingFilterWorker
	{
		// Token: 0x06003732 RID: 14130 RVA: 0x001D84D8 File Offset: 0x001D68D8
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x001D84FC File Offset: 0x001D68FC
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.ingestible != null && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.None;
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x001D8534 File Offset: 0x001D6934
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
