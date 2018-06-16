using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020004F0 RID: 1264
	public static class RestraintsUtility
	{
		// Token: 0x060016A8 RID: 5800 RVA: 0x000C8D58 File Offset: 0x000C7158
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

		// Token: 0x060016A9 RID: 5801 RVA: 0x000C8DE4 File Offset: 0x000C71E4
		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
