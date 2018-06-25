using System;
using Verse;

namespace RimWorld
{
	public class SpecialThingFilterWorker_Fresh : SpecialThingFilterWorker
	{
		public SpecialThingFilterWorker_Fresh()
		{
		}

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

		public override bool CanEverMatch(ThingDef def)
		{
			return def.GetCompProperties<CompProperties_Rottable>() != null || def.IsIngestible;
		}

		public override bool AlwaysMatches(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return (compProperties != null && compProperties.rotDestroys) || (compProperties == null && def.IsIngestible);
		}
	}
}
