using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000168 RID: 360
	public static class GatheringsUtility
	{
		// Token: 0x06000766 RID: 1894 RVA: 0x00049754 File Offset: 0x00047B54
		public static bool ShouldGuestKeepAttendingGathering(Pawn p)
		{
			return !p.Downed && (p.needs == null || !p.needs.food.Starving) && p.health.hediffSet.BleedRateTotal <= 0f && p.needs.rest.CurCategory < RestCategory.Exhausted && !p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff(false) && p.Awake() && !p.InAggroMentalState && !p.IsPrisoner;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00049834 File Offset: 0x00047C34
		public static bool AnyLordJobPreventsNewGatherings(Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob lordJob = lords[i].LordJob;
				if (!lordJob.AllowStartNewGatherings)
				{
					return true;
				}
			}
			return false;
		}
	}
}
