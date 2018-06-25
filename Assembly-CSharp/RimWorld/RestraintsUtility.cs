using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class RestraintsUtility
	{
		public static bool InRestraints(Pawn pawn)
		{
			bool result;
			if (!pawn.Spawned)
			{
				result = false;
			}
			else if (pawn.HostFaction == null)
			{
				result = false;
			}
			else
			{
				Lord lord = pawn.GetLord();
				result = ((lord == null || lord.LordJob == null || !lord.LordJob.NeverInRestraints) && (pawn.guest == null || !pawn.guest.Released));
			}
			return result;
		}

		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
