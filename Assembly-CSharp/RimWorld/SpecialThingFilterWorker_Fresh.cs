using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099B RID: 2459
	public class SpecialThingFilterWorker_Fresh : SpecialThingFilterWorker
	{
		// Token: 0x0600372C RID: 14124 RVA: 0x001D8334 File Offset: 0x001D6734
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			bool result;
			if (compRottable == null)
			{
				result = t.def.IsIngestible;
			}
			else
			{
				result = (compRottable.Stage == RotStage.Fresh);
			}
			return result;
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x001D8380 File Offset: 0x001D6780
		public override bool CanEverMatch(ThingDef def)
		{
			return def.GetCompProperties<CompProperties_Rottable>() != null || def.IsIngestible;
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x001D83AC File Offset: 0x001D67AC
		public override bool AlwaysMatches(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return (compProperties != null && compProperties.rotDestroys) || (compProperties == null && def.IsIngestible);
		}
	}
}
