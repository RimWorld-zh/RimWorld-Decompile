using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F8 RID: 2552
	public class LordToil_ExitMapTraderFighting : LordToil
	{
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06003949 RID: 14665 RVA: 0x001E722C File Offset: 0x001E562C
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x0600394A RID: 14666 RVA: 0x001E7244 File Offset: 0x001E5644
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600394B RID: 14667 RVA: 0x001E725C File Offset: 0x001E565C
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
