using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099A RID: 2458
	public class SpecialThingFilterWorker_Rotten : SpecialThingFilterWorker
	{
		// Token: 0x0600372B RID: 14123 RVA: 0x001D8388 File Offset: 0x001D6788
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && !compRottable.PropsRot.rotDestroys && compRottable.Stage != RotStage.Fresh;
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x001D83D0 File Offset: 0x001D67D0
		public override bool CanEverMatch(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return compProperties != null && !compProperties.rotDestroys;
		}
	}
}
