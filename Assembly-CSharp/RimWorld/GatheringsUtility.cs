using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class GatheringsUtility
	{
		public static bool ShouldGuestKeepAttendingGathering(Pawn p)
		{
			return (byte)((!p.Downed) ? ((p.needs == null || !p.needs.food.Starving) ? ((!(p.health.hediffSet.BleedRateTotal > 0.0)) ? (((int)p.needs.rest.CurCategory < 3) ? ((!p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff(false)) ? (p.Awake() ? ((!p.InAggroMentalState) ? ((!p.IsPrisoner) ? 1 : 0) : 0) : 0) : 0) : 0) : 0) : 0) : 0) != 0;
		}

		public static bool AnyLordJobPreventsNewGatherings(Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < lords.Count)
				{
					LordJob lordJob = lords[num].LordJob;
					if (!lordJob.AllowStartNewGatherings)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
