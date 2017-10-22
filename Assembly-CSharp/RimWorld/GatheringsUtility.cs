using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class GatheringsUtility
	{
		public static bool ShouldGuestKeepAttendingGathering(Pawn p)
		{
			if (p.Downed)
			{
				return false;
			}
			if (p.needs != null && p.needs.food.Starving)
			{
				return false;
			}
			if (p.health.hediffSet.BleedRateTotal > 0.0)
			{
				return false;
			}
			if ((int)p.needs.rest.CurCategory >= 3)
			{
				return false;
			}
			if (p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff(false))
			{
				return false;
			}
			if (!p.Awake())
			{
				return false;
			}
			if (p.InAggroMentalState)
			{
				return false;
			}
			if (p.IsPrisoner)
			{
				return false;
			}
			return true;
		}

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
