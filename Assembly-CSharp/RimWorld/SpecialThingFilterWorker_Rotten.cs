using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000996 RID: 2454
	public class SpecialThingFilterWorker_Rotten : SpecialThingFilterWorker
	{
		// Token: 0x06003724 RID: 14116 RVA: 0x001D8584 File Offset: 0x001D6984
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && !compRottable.PropsRot.rotDestroys && compRottable.Stage != RotStage.Fresh;
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x001D85CC File Offset: 0x001D69CC
		public override bool CanEverMatch(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return compProperties != null && !compProperties.rotDestroys;
		}
	}
}
