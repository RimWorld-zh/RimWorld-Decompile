using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020004F0 RID: 1264
	public static class RestraintsUtility
	{
		// Token: 0x060016A9 RID: 5801 RVA: 0x000C8DAC File Offset: 0x000C71AC
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

		// Token: 0x060016AA RID: 5802 RVA: 0x000C8E38 File Offset: 0x000C7238
		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
