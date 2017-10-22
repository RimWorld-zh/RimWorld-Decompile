#define ENABLE_PROFILER
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_HaulCorpses : WorkGiver_Haul
	{
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling();
		}

		public override bool ShouldSkip(Pawn pawn)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling().Count == 0;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (!(t is Corpse))
			{
				result = null;
			}
			else
			{
				Profiler.BeginSample("PawnCanAutomaticallyHaulFast");
				if (!HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, forced))
				{
					Profiler.EndSample();
					result = null;
				}
				else
				{
					Profiler.EndSample();
					result = HaulAIUtility.HaulToStorageJob(pawn, t);
				}
			}
			return result;
		}
	}
}
