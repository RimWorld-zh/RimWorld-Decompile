using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F9 RID: 2553
	public class LordToil_ExitMapTraderFighting : LordToil
	{
		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06003948 RID: 14664 RVA: 0x001E6AC0 File Offset: 0x001E4EC0
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06003949 RID: 14665 RVA: 0x001E6AD8 File Offset: 0x001E4ED8
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x001E6AF0 File Offset: 0x001E4EF0
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
