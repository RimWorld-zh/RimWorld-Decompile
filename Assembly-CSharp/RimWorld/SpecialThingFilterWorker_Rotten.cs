using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099A RID: 2458
	public class SpecialThingFilterWorker_Rotten : SpecialThingFilterWorker
	{
		// Token: 0x06003729 RID: 14121 RVA: 0x001D82B4 File Offset: 0x001D66B4
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && !compRottable.PropsRot.rotDestroys && compRottable.Stage != RotStage.Fresh;
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x001D82FC File Offset: 0x001D66FC
		public override bool CanEverMatch(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return compProperties != null && !compProperties.rotDestroys;
		}
	}
}
