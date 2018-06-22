using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020004EC RID: 1260
	public static class RestraintsUtility
	{
		// Token: 0x060016A0 RID: 5792 RVA: 0x000C8DA4 File Offset: 0x000C71A4
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

		// Token: 0x060016A1 RID: 5793 RVA: 0x000C8E30 File Offset: 0x000C7230
		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
