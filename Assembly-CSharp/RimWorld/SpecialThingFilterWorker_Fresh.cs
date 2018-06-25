using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000999 RID: 2457
	public class SpecialThingFilterWorker_Fresh : SpecialThingFilterWorker
	{
		// Token: 0x0600372B RID: 14123 RVA: 0x001D8744 File Offset: 0x001D6B44
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

		// Token: 0x0600372C RID: 14124 RVA: 0x001D8790 File Offset: 0x001D6B90
		public override bool CanEverMatch(ThingDef def)
		{
			return def.GetCompProperties<CompProperties_Rottable>() != null || def.IsIngestible;
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x001D87BC File Offset: 0x001D6BBC
		public override bool AlwaysMatches(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return (compProperties != null && compProperties.rotDestroys) || (compProperties == null && def.IsIngestible);
		}
	}
}
