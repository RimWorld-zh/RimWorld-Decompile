using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000998 RID: 2456
	public class SpecialThingFilterWorker_Rotten : SpecialThingFilterWorker
	{
		// Token: 0x06003728 RID: 14120 RVA: 0x001D8998 File Offset: 0x001D6D98
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && !compRottable.PropsRot.rotDestroys && compRottable.Stage != RotStage.Fresh;
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x001D89E0 File Offset: 0x001D6DE0
		public override bool CanEverMatch(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return compProperties != null && !compProperties.rotDestroys;
		}
	}
}
