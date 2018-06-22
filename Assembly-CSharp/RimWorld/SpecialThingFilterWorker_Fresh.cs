using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000997 RID: 2455
	public class SpecialThingFilterWorker_Fresh : SpecialThingFilterWorker
	{
		// Token: 0x06003727 RID: 14119 RVA: 0x001D8604 File Offset: 0x001D6A04
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

		// Token: 0x06003728 RID: 14120 RVA: 0x001D8650 File Offset: 0x001D6A50
		public override bool CanEverMatch(ThingDef def)
		{
			return def.GetCompProperties<CompProperties_Rottable>() != null || def.IsIngestible;
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x001D867C File Offset: 0x001D6A7C
		public override bool AlwaysMatches(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return (compProperties != null && compProperties.rotDestroys) || (compProperties == null && def.IsIngestible);
		}
	}
}
