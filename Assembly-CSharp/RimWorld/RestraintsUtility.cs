using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020004EE RID: 1262
	public static class RestraintsUtility
	{
		// Token: 0x060016A3 RID: 5795 RVA: 0x000C90F4 File Offset: 0x000C74F4
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

		// Token: 0x060016A4 RID: 5796 RVA: 0x000C9180 File Offset: 0x000C7580
		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
