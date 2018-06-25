using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000148 RID: 328
	public abstract class WorkGiver_Haul : WorkGiver_Scanner
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x000460B0 File Offset: 0x000444B0
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000460C8 File Offset: 0x000444C8
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000460F0 File Offset: 0x000444F0
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling().Count == 0;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00046120 File Offset: 0x00044520
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Profiler.BeginSample("PawnCanAutomaticallyHaulFast");
			Job result;
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
			return result;
		}
	}
}
