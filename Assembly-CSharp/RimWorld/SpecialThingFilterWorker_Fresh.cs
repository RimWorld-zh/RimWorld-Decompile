using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000999 RID: 2457
	public class SpecialThingFilterWorker_Fresh : SpecialThingFilterWorker
	{
		// Token: 0x0600372B RID: 14123 RVA: 0x001D8A18 File Offset: 0x001D6E18
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

		// Token: 0x0600372C RID: 14124 RVA: 0x001D8A64 File Offset: 0x001D6E64
		public override bool CanEverMatch(ThingDef def)
		{
			return def.GetCompProperties<CompProperties_Rottable>() != null || def.IsIngestible;
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x001D8A90 File Offset: 0x001D6E90
		public override bool AlwaysMatches(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return (compProperties != null && compProperties.rotDestroys) || (compProperties == null && def.IsIngestible);
		}
	}
}
