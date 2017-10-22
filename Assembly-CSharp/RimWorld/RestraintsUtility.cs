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
				result = ((byte)((lord == null || lord.LordJob == null || !lord.LordJob.NeverInRestraints) ? 1 : 0) != 0);
			}
			return result;
		}

		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
