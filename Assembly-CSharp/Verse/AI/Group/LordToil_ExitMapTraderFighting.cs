using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F9 RID: 2553
	public class LordToil_ExitMapTraderFighting : LordToil
	{
		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600394A RID: 14666 RVA: 0x001E6B94 File Offset: 0x001E4F94
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x001E6BAC File Offset: 0x001E4FAC
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x001E6BC4 File Offset: 0x001E4FC4
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				TraderCaravanRole traderCaravanRole = pawn.GetTraderCaravanRole();
				if (traderCaravanRole == TraderCaravanRole.Carrier || traderCaravanRole == TraderCaravanRole.Chattel)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest);
					pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
				}
			}
		}
	}
}
